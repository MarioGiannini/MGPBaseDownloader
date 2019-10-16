namespace MGPBaseDownloader
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.tbURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDestFolder = new System.Windows.Forms.TextBox();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnPickFolder = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tbResults = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbURL
            // 
            this.tbURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbURL.Location = new System.Drawing.Point(15, 29);
            this.tbURL.Name = "tbURL";
            this.tbURL.Size = new System.Drawing.Size(521, 22);
            this.tbURL.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "URL to root gallery";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination Folder";
            // 
            // tbDestFolder
            // 
            this.tbDestFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDestFolder.Location = new System.Drawing.Point(15, 85);
            this.tbDestFolder.Name = "tbDestFolder";
            this.tbDestFolder.Size = new System.Drawing.Size(489, 22);
            this.tbDestFolder.TabIndex = 2;
            // 
            // btnPickFolder
            // 
            this.btnPickFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPickFolder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPickFolder.Image = global::MGPBaseDownloader.Properties.Resources.Folder_24x;
            this.btnPickFolder.Location = new System.Drawing.Point(506, 84);
            this.btnPickFolder.Name = "btnPickFolder";
            this.btnPickFolder.Size = new System.Drawing.Size(30, 26);
            this.btnPickFolder.TabIndex = 4;
            this.btnPickFolder.UseVisualStyleBackColor = true;
            this.btnPickFolder.Click += new System.EventHandler(this.btnPickFolder_Click);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(15, 126);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 5;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(96, 129);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            this.lblStatus.TabIndex = 6;
            // 
            // tbResults
            // 
            this.tbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResults.Location = new System.Drawing.Point(15, 155);
            this.tbResults.Multiline = true;
            this.tbResults.Name = "tbResults";
            this.tbResults.ReadOnly = true;
            this.tbResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResults.Size = new System.Drawing.Size(521, 221);
            this.tbResults.TabIndex = 7;
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 392);
            this.Controls.Add(this.tbResults);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnPickFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDestFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbURL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "formMain";
            this.Text = "MG PBase Downloaded";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.Load += new System.EventHandler(this.formMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDestFolder;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.Button btnPickFolder;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox tbResults;
    }
}

