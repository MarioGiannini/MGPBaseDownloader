using System;
using System.Windows.Forms;

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
    public partial class formMain : Form
    {
        PBaseDownloader DownLoader;

        public formMain()
        {
            DownLoader = new PBaseDownloader(@"SOFTWARE\Mario Giannini\MGPBaseDownloader");
            InitializeComponent();
        }

        private void btnPickFolder_Click(object sender, EventArgs e)
        {
            dlgFolder.SelectedPath = DownLoader.DestinationFolder;
            if (dlgFolder.ShowDialog() == DialogResult.OK)
            {
                tbDestFolder.Text = dlgFolder.SelectedPath;
                DownLoader.SetPaths(tbURL.Text, tbDestFolder.Text);
            }
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if( DownLoader.IsRunning )
            {
                if (MessageBox.Show("Are you sure you want to cancel processing?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;
            }
            else
                DownLoader.SetPaths(tbURL.Text, tbDestFolder.Text);
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            tbURL.Text = DownLoader.SourceURL;
            tbDestFolder.Text = DownLoader.DestinationFolder;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if( !DownLoader.IsRunning )
            {
                DownLoader.SetPaths(tbURL.Text, tbDestFolder.Text);
                btnGo.Text = "Cancel";
                tbURL.Enabled = false;
                tbDestFolder.Enabled = false;
                btnPickFolder.Enabled = false;

                DownLoader.ProcessUrl (lblStatus, tbResults);

                btnGo.Text = "Go";
                tbURL.Enabled = true;
                tbDestFolder.Enabled = true;
                btnPickFolder.Enabled = true;
            }
            else
            {
                if( MessageBox.Show( "Are you sure you want to cancel processing?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
                    DownLoader.Cancel();
            }
        }
    }
}
