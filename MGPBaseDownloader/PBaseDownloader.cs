using System;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Microsoft.Win32;
using HtmlAgilityPack;
/*
 Copyright 2019 Mario Giannini

Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, 
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or 
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

 */ 

namespace MGPBaseDownloader
{
    class PBaseDownloader
    {
        private String fDestinationFolder;
        private String fSourceURL;
        private int fTotalItems, fProcessedItems, fProcessedFolders;
        private String fRegistryKey;
        Boolean fIsRunning, fWasCancelled;

        public PBaseDownloader(String aRegistryKey)
        {
            fRegistryKey = aRegistryKey;
            fIsRunning = false;
            fWasCancelled = false;
            LoadSettings();
        }

        public String SourceURL
        {
            get { return fSourceURL; }
        }
        public String DestinationFolder
        {
            get { return fDestinationFolder; }
        }

        public Boolean IsRunning
        {
            get { return fIsRunning; }
        }
        public Boolean WasCancelled
        {
            get { return fWasCancelled; }
        }

        public void Cancel()
        {
            fWasCancelled = true;
        }

        private Boolean SetStatus(Label lblStatus, String Status) // Returns true as long as not cancelled, and updates status control
        {
            if (fWasCancelled == false)
            {
                if (lblStatus != null)
                    lblStatus.Text = Status;
                Application.DoEvents();
            }
            return !fWasCancelled;
        }

        public void SaveSettings()  // Save data to the registry
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(fRegistryKey))   // IDisposable
            {
                key.SetValue("PBaseDownloader.DestinationFolder", DestinationFolder);
                key.SetValue("PBaseDownloader.SourceURL", SourceURL);
                key.Close();
            }
        }
        private void LoadSettings() // Load data from the registry
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(fRegistryKey))  // IDisposable
            {
                if (key != null)
                {
                    fDestinationFolder = key.GetValue("PBaseDownloader.DestinationFolder", "").ToString();
                    fSourceURL = key.GetValue("PBaseDownloader.SourceURL", "").ToString();
                    key.Close();
                }
            }
        }

        public void SetPaths(String aSourceURL, String aDestinationFolder)
        {
            fSourceURL = aSourceURL;
            fDestinationFolder = aDestinationFolder;
            if (!fDestinationFolder.EndsWith("\\"))
                fDestinationFolder += "\\";
            SaveSettings();
        }

        private String FilenameSafe(String Filename)    // Returns a filename safe for Windows OS
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                Filename = Filename.Replace(c, '-');
            return Filename;
        }

        private String URLAppend(String URL, String Parameter)  // appends a parameter to URL ir needed (pbase specific example)
        {
            if( URL.IndexOf( "&" + Parameter ) < 0 && URL.IndexOf( "?" + Parameter ) < 0 )
                return URL + "&" + Parameter; // PBase doesn't seem to care about the ? seperator
            else
                return URL;
        }

        private String ExtractString( String Src, String Attribute )    // Extract quoted string from attribute of HTML tag
        {
            int StartQ = Src.IndexOf(Attribute);
            if(StartQ > -1 )
            {
                StartQ = Src.IndexOf('"', StartQ + Attribute.Length);
                if (StartQ > -1)
                {
                    int EndQ = Src.IndexOf('"', StartQ + 1);
                    return (Src.Substring(StartQ+1, EndQ - StartQ -1));
                }
            }
            return "";
        }
        
        private String ExtractUserDesc( String Page )   // Extracts user description from a gallery page
        {
            int TheStart = Page.IndexOf("<!-- BEGIN user desc -->"), TheEnd = Page.IndexOf("<!-- END user desc -->");
            if (TheStart > -1 && TheEnd > TheStart)
                return Page.Substring(TheStart + 24, TheEnd - TheStart - 24);
            return "";
        }


        public void ProcessUrlEx(int Level, String DestFolder, String url, Label Status, TextBox Results) // Recursive processing of a URL
        {
            String ItemName, ItemImage, ItemLink;
            int ItemIndex = 0;
            HtmlAgilityPack.HtmlNodeCollection ItemNodes;
            String SaveFile;

            if ( fWasCancelled )
                return;

            using (WebClient Client = new WebClient()) // IDisposable
            {
                url = URLAppend(url, "page=all");
                String Page = Client.DownloadString(url);
                HtmlAgilityPack.HtmlDocument Doc = new HtmlAgilityPack.HtmlDocument();
                Doc.LoadHtml(Page);
                ItemNodes = Doc.DocumentNode.SelectNodes("//td[@class='thumbnail']");
                if (ItemNodes == null)
                    Results.AppendText( ">> No images or folders\r\n");
                else
                {
                    fTotalItems += ItemNodes.Count;
                    File.WriteAllText(DestFolder + "Description.txt", ExtractUserDesc(Page) );  // Save the Description text to txt file

                    // Iterate items on the page (assume < 1000 items)
                    foreach (HtmlNode Node in ItemNodes)
                    {
                        ItemIndex++;
                        ItemName = FilenameSafe( ItemIndex.ToString("D3") + "-" + ExtractString( Node.InnerHtml, "alt=") ); // force 3-digit prefix to maintian order
                        ItemImage = ExtractString( Node.InnerHtml, "src=");
                        ItemLink = ExtractString( Node.InnerHtml, "href=");

                        if( ItemLink.IndexOf( SourceURL + "/image/") > -1 ) // Is the item a link to an image?  If so, download and save data
                        {
                            //Load the Image page
                            SaveFile = DestFolder + ItemIndex.ToString(  "D3" ) + ".jpg";
                            HtmlAgilityPack.HtmlDocument ImageDoc = new HtmlAgilityPack.HtmlDocument();
                            if (!File.Exists(SaveFile))
                            {
                                ItemLink = ItemLink + "/original&exif=Y";
                                Page = Client.DownloadString( ItemLink );
                                ImageDoc.LoadHtml( Page );
                                HtmlNode jpgnode = ImageDoc.DocumentNode.SelectSingleNode("//div[@id='imgdiv']");
                                if (jpgnode != null)
                                {
                                    String jpgLink = ExtractString(jpgnode.InnerHtml, "src=");
                                    Client.DownloadFile( jpgLink, SaveFile );
                                }
                            }
                            
                            // Iterate the Title and EXIF data if available, and save to txt file
                            SaveFile = DestFolder + ItemIndex.ToString( "D3" ) + ".txt";
                            if (!File.Exists(SaveFile))
                            {
                                String EXIF = "";
                                HtmlNode TitleNode = ImageDoc.DocumentNode.SelectSingleNode("//span[@class='title']");
                                if (TitleNode != null)
                                    EXIF = "Title=" + TitleNode.InnerText + "\n";
                                HtmlNodeCollection ExifNodes = ImageDoc.DocumentNode.SelectNodes("//td[@class='lid']");
                                Boolean first = true;
                                if (ExifNodes != null)
                                {
                                    foreach (HtmlNode ExifNode in ExifNodes)
                                    {
                                        if (first)
                                            EXIF = EXIF + ExifNode.InnerText;
                                        else
                                            EXIF = EXIF + "=" + ExifNode.InnerText + "\n";
                                        first = !first;
                                    }
                                }
                                File.WriteAllText(SaveFile, EXIF);
                            }
                        }
                        else // Is the item a link to a gallery or image page?  If so, explore it
                        {
                            Results.AppendText("Processing folder " + ItemLink.Replace(SourceURL, "") + "\r\n");
                            ItemLink = URLAppend(ItemLink, "page=all" );
                            if (!Directory.Exists(DestFolder + ItemName))   // Create folder and store thumbnail
                                Directory.CreateDirectory(DestFolder + ItemName);
                            Client.DownloadFile( ItemImage, DestFolder + ItemName + "/Thumbnail.jpg");
                            fProcessedFolders++;
                            ProcessUrlEx(Level + 1, DestFolder + ItemName + '\\', ItemLink, Status, Results);
                        }
                        
                        fProcessedItems++;
                        if (!SetStatus( Status, "Processed " + fProcessedItems.ToString() + " of " + fTotalItems.ToString() + " discovered items"))
                            break;
                    }
                }
            }
        }

        public Boolean ProcessUrl( Label Status, TextBox Results )  // Setup, call recursive search, and report final details
        {
            String url = SourceURL;
            fTotalItems = 0;
            fProcessedItems = 0;
            fProcessedFolders = 0;
            fIsRunning = true;
            fWasCancelled = false;

            Results.Clear();
            if (!url.Contains("/root&page=all"))
                url = url + "/root&page=all";

            try
            { 
                ProcessUrlEx(0, this.fDestinationFolder, url, Status, Results);
            }
            catch ( Exception Ex )
            {
                MessageBox.Show(Ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Results.AppendText( "Exception: " + Ex.Message + "\r\n" );
            }
            fIsRunning = false;

            Results.AppendText( fProcessedItems.ToString() + " images processed\r\n" );
            Results.AppendText(fProcessedFolders.ToString() + " folders processed\r\n" );
            if (fWasCancelled)
                Results.AppendText( "Processing Cancelled\r\n" );
            else
                Results.AppendText( "Processing Completed\r\n" );
            SetStatus(Status, "Done");
            return !fWasCancelled;
        }
    }
}
