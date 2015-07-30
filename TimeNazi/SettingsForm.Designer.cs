namespace TimeNazi
{
    partial class SettingsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbStartWithWindows = new System.Windows.Forms.CheckBox();
            this.lblClockOpacity = new System.Windows.Forms.Label();
            this.tbClockOpacity = new System.Windows.Forms.TrackBar();
            this.label11 = new System.Windows.Forms.Label();
            this.nudRestTime = new System.Windows.Forms.NumericUpDown();
            this.nudSnoozeTime = new System.Windows.Forms.NumericUpDown();
            this.nudWorkTime = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStatusLabel = new System.Windows.Forms.Label();
            this.lblGoogleStatus = new System.Windows.Forms.Label();
            this.btnBrowseGoogleKeyFile = new System.Windows.Forms.Button();
            this.txtGoogleKeyFile = new System.Windows.Forms.TextBox();
            this.lblKeyFileLabel = new System.Windows.Forms.Label();
            this.btnTestGoogleConnection = new System.Windows.Forms.Button();
            this.cbUseGoogleDrive = new System.Windows.Forms.CheckBox();
            this.btnAuthorizeWithGoogle = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDefaults = new System.Windows.Forms.Button();
            this.ofdDatabaseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fbdBackgroundImageLocationDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblBackgroundImagesStatus = new System.Windows.Forms.Label();
            this.btnCheckBackgroundImages = new System.Windows.Forms.Button();
            this.btnDownloadBackgroundImages = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.lnkCancelTask = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbClockOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSnoozeTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWorkTime)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbStartWithWindows);
            this.groupBox1.Controls.Add(this.lblClockOpacity);
            this.groupBox1.Controls.Add(this.tbClockOpacity);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.nudRestTime);
            this.groupBox1.Controls.Add(this.nudSnoozeTime);
            this.groupBox1.Controls.Add(this.nudWorkTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 152);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // cbStartWithWindows
            // 
            this.cbStartWithWindows.AutoSize = true;
            this.cbStartWithWindows.Location = new System.Drawing.Point(9, 127);
            this.cbStartWithWindows.Name = "cbStartWithWindows";
            this.cbStartWithWindows.Size = new System.Drawing.Size(207, 17);
            this.cbStartWithWindows.TabIndex = 15;
            this.cbStartWithWindows.Text = "Start with Windows (current user only!)";
            this.cbStartWithWindows.UseVisualStyleBackColor = true;
            // 
            // lblClockOpacity
            // 
            this.lblClockOpacity.AutoSize = true;
            this.lblClockOpacity.Location = new System.Drawing.Point(251, 95);
            this.lblClockOpacity.Name = "lblClockOpacity";
            this.lblClockOpacity.Size = new System.Drawing.Size(21, 13);
            this.lblClockOpacity.TabIndex = 14;
            this.lblClockOpacity.Text = "0%";
            // 
            // tbClockOpacity
            // 
            this.tbClockOpacity.LargeChange = 10;
            this.tbClockOpacity.Location = new System.Drawing.Point(83, 92);
            this.tbClockOpacity.Maximum = 100;
            this.tbClockOpacity.Name = "tbClockOpacity";
            this.tbClockOpacity.Size = new System.Drawing.Size(162, 45);
            this.tbClockOpacity.TabIndex = 13;
            this.tbClockOpacity.TickFrequency = 10;
            this.tbClockOpacity.Scroll += new System.EventHandler(this.tbClockOpacity_Scroll);
            this.tbClockOpacity.ValueChanged += new System.EventHandler(this.tbClockOpacity_ValueChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 95);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Clock opacity";
            // 
            // nudRestTime
            // 
            this.nudRestTime.Location = new System.Drawing.Point(83, 66);
            this.nudRestTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudRestTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRestTime.Name = "nudRestTime";
            this.nudRestTime.Size = new System.Drawing.Size(58, 20);
            this.nudRestTime.TabIndex = 11;
            this.nudRestTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudSnoozeTime
            // 
            this.nudSnoozeTime.Location = new System.Drawing.Point(83, 40);
            this.nudSnoozeTime.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudSnoozeTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSnoozeTime.Name = "nudSnoozeTime";
            this.nudSnoozeTime.Size = new System.Drawing.Size(58, 20);
            this.nudSnoozeTime.TabIndex = 10;
            this.nudSnoozeTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudWorkTime
            // 
            this.nudWorkTime.Location = new System.Drawing.Point(83, 14);
            this.nudWorkTime.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.nudWorkTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWorkTime.Name = "nudWorkTime";
            this.nudWorkTime.Size = new System.Drawing.Size(58, 20);
            this.nudWorkTime.TabIndex = 9;
            this.nudWorkTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(141, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "(minutes)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Work time";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(141, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "(minutes)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Snooze time";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "(minutes)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Rest time";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStatusLabel);
            this.groupBox2.Controls.Add(this.lblGoogleStatus);
            this.groupBox2.Controls.Add(this.btnBrowseGoogleKeyFile);
            this.groupBox2.Controls.Add(this.txtGoogleKeyFile);
            this.groupBox2.Controls.Add(this.lblKeyFileLabel);
            this.groupBox2.Controls.Add(this.btnTestGoogleConnection);
            this.groupBox2.Controls.Add(this.cbUseGoogleDrive);
            this.groupBox2.Controls.Add(this.btnAuthorizeWithGoogle);
            this.groupBox2.Location = new System.Drawing.Point(12, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(373, 120);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Database";
            // 
            // lblStatusLabel
            // 
            this.lblStatusLabel.AutoSize = true;
            this.lblStatusLabel.Location = new System.Drawing.Point(6, 67);
            this.lblStatusLabel.Name = "lblStatusLabel";
            this.lblStatusLabel.Size = new System.Drawing.Size(37, 13);
            this.lblStatusLabel.TabIndex = 3;
            this.lblStatusLabel.Text = "Status";
            // 
            // lblGoogleStatus
            // 
            this.lblGoogleStatus.AutoSize = true;
            this.lblGoogleStatus.Location = new System.Drawing.Point(50, 67);
            this.lblGoogleStatus.Name = "lblGoogleStatus";
            this.lblGoogleStatus.Size = new System.Drawing.Size(51, 13);
            this.lblGoogleStatus.TabIndex = 4;
            this.lblGoogleStatus.Text = "unknown";
            // 
            // btnBrowseGoogleKeyFile
            // 
            this.btnBrowseGoogleKeyFile.Location = new System.Drawing.Point(292, 38);
            this.btnBrowseGoogleKeyFile.Name = "btnBrowseGoogleKeyFile";
            this.btnBrowseGoogleKeyFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseGoogleKeyFile.TabIndex = 22;
            this.btnBrowseGoogleKeyFile.Text = "Browse...";
            this.btnBrowseGoogleKeyFile.UseVisualStyleBackColor = true;
            this.btnBrowseGoogleKeyFile.Click += new System.EventHandler(this.btnBrowseGoogleKeyFile_Click);
            // 
            // txtGoogleKeyFile
            // 
            this.txtGoogleKeyFile.Location = new System.Drawing.Point(53, 40);
            this.txtGoogleKeyFile.Name = "txtGoogleKeyFile";
            this.txtGoogleKeyFile.Size = new System.Drawing.Size(233, 20);
            this.txtGoogleKeyFile.TabIndex = 21;
            // 
            // lblKeyFileLabel
            // 
            this.lblKeyFileLabel.AutoSize = true;
            this.lblKeyFileLabel.Location = new System.Drawing.Point(6, 43);
            this.lblKeyFileLabel.Name = "lblKeyFileLabel";
            this.lblKeyFileLabel.Size = new System.Drawing.Size(41, 13);
            this.lblKeyFileLabel.TabIndex = 20;
            this.lblKeyFileLabel.Text = "Key file";
            // 
            // btnTestGoogleConnection
            // 
            this.btnTestGoogleConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestGoogleConnection.Enabled = false;
            this.btnTestGoogleConnection.Location = new System.Drawing.Point(292, 91);
            this.btnTestGoogleConnection.Name = "btnTestGoogleConnection";
            this.btnTestGoogleConnection.Size = new System.Drawing.Size(75, 23);
            this.btnTestGoogleConnection.TabIndex = 19;
            this.btnTestGoogleConnection.Text = "Test";
            this.btnTestGoogleConnection.UseVisualStyleBackColor = true;
            this.btnTestGoogleConnection.Click += new System.EventHandler(this.btnTestGoogleConnection_Click);
            // 
            // cbUseGoogleDrive
            // 
            this.cbUseGoogleDrive.AutoSize = true;
            this.cbUseGoogleDrive.Location = new System.Drawing.Point(9, 19);
            this.cbUseGoogleDrive.Name = "cbUseGoogleDrive";
            this.cbUseGoogleDrive.Size = new System.Drawing.Size(110, 17);
            this.cbUseGoogleDrive.TabIndex = 18;
            this.cbUseGoogleDrive.Text = "Use Google Drive";
            this.cbUseGoogleDrive.UseVisualStyleBackColor = true;
            this.cbUseGoogleDrive.CheckedChanged += new System.EventHandler(this.cbUseGoogleDrive_CheckedChanged);
            // 
            // btnAuthorizeWithGoogle
            // 
            this.btnAuthorizeWithGoogle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAuthorizeWithGoogle.Enabled = false;
            this.btnAuthorizeWithGoogle.Location = new System.Drawing.Point(211, 91);
            this.btnAuthorizeWithGoogle.Name = "btnAuthorizeWithGoogle";
            this.btnAuthorizeWithGoogle.Size = new System.Drawing.Size(75, 23);
            this.btnAuthorizeWithGoogle.TabIndex = 17;
            this.btnAuthorizeWithGoogle.Text = "Authorize";
            this.btnAuthorizeWithGoogle.UseVisualStyleBackColor = true;
            this.btnAuthorizeWithGoogle.Click += new System.EventHandler(this.btnAuthorizeWithGoogle_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(227, 387);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(310, 387);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDefaults
            // 
            this.btnDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDefaults.Location = new System.Drawing.Point(12, 387);
            this.btnDefaults.Name = "btnDefaults";
            this.btnDefaults.Size = new System.Drawing.Size(75, 23);
            this.btnDefaults.TabIndex = 2;
            this.btnDefaults.Text = "Defaults";
            this.btnDefaults.UseVisualStyleBackColor = true;
            this.btnDefaults.Click += new System.EventHandler(this.btnDefaults_Click);
            // 
            // ofdDatabaseFileDialog
            // 
            this.ofdDatabaseFileDialog.CheckFileExists = false;
            this.ofdDatabaseFileDialog.DefaultExt = "oakey";
            this.ofdDatabaseFileDialog.Filter = "OAuth2 key files (*.oakey)|*.oakey|All files (*.*)|*.*";
            this.ofdDatabaseFileDialog.InitialDirectory = "C:\\";
            this.ofdDatabaseFileDialog.RestoreDirectory = true;
            this.ofdDatabaseFileDialog.Title = "Select key file";
            // 
            // fbdBackgroundImageLocationDialog
            // 
            this.fbdBackgroundImageLocationDialog.Description = "Select the location of the background images";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lnkCancelTask);
            this.groupBox3.Controls.Add(this.lblBackgroundImagesStatus);
            this.groupBox3.Controls.Add(this.btnCheckBackgroundImages);
            this.groupBox3.Controls.Add(this.btnDownloadBackgroundImages);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(12, 296);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(373, 64);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Background images";
            // 
            // lblBackgroundImagesStatus
            // 
            this.lblBackgroundImagesStatus.AutoSize = true;
            this.lblBackgroundImagesStatus.Location = new System.Drawing.Point(50, 16);
            this.lblBackgroundImagesStatus.Name = "lblBackgroundImagesStatus";
            this.lblBackgroundImagesStatus.Size = new System.Drawing.Size(51, 13);
            this.lblBackgroundImagesStatus.TabIndex = 8;
            this.lblBackgroundImagesStatus.Text = "unknown";
            // 
            // btnCheckBackgroundImages
            // 
            this.btnCheckBackgroundImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckBackgroundImages.Location = new System.Drawing.Point(211, 35);
            this.btnCheckBackgroundImages.Name = "btnCheckBackgroundImages";
            this.btnCheckBackgroundImages.Size = new System.Drawing.Size(75, 23);
            this.btnCheckBackgroundImages.TabIndex = 7;
            this.btnCheckBackgroundImages.Text = "Check";
            this.btnCheckBackgroundImages.UseVisualStyleBackColor = true;
            this.btnCheckBackgroundImages.Click += new System.EventHandler(this.btnCheckBackgroundImages_Click);
            // 
            // btnDownloadBackgroundImages
            // 
            this.btnDownloadBackgroundImages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownloadBackgroundImages.Location = new System.Drawing.Point(292, 35);
            this.btnDownloadBackgroundImages.Name = "btnDownloadBackgroundImages";
            this.btnDownloadBackgroundImages.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadBackgroundImages.TabIndex = 6;
            this.btnDownloadBackgroundImages.Text = "Download";
            this.btnDownloadBackgroundImages.UseVisualStyleBackColor = true;
            this.btnDownloadBackgroundImages.Click += new System.EventHandler(this.btnDownloadBackgroundImages_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Status";
            // 
            // lnkCancelTask
            // 
            this.lnkCancelTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkCancelTask.AutoSize = true;
            this.lnkCancelTask.Location = new System.Drawing.Point(165, 40);
            this.lnkCancelTask.Name = "lnkCancelTask";
            this.lnkCancelTask.Size = new System.Drawing.Size(40, 13);
            this.lnkCancelTask.TabIndex = 9;
            this.lnkCancelTask.TabStop = true;
            this.lnkCancelTask.Text = "Cancel";
            this.lnkCancelTask.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkCancelTask.Visible = false;
            this.lnkCancelTask.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCancelTask_LinkClicked);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(395, 422);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDefaults);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbClockOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRestTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSnoozeTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWorkTime)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudRestTime;
        private System.Windows.Forms.NumericUpDown nudSnoozeTime;
        private System.Windows.Forms.NumericUpDown nudWorkTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDefaults;
        private System.Windows.Forms.TrackBar tbClockOpacity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblClockOpacity;
        private System.Windows.Forms.OpenFileDialog ofdDatabaseFileDialog;
        private System.Windows.Forms.FolderBrowserDialog fbdBackgroundImageLocationDialog;
        private System.Windows.Forms.CheckBox cbStartWithWindows;
        private System.Windows.Forms.Button btnAuthorizeWithGoogle;
        private System.Windows.Forms.CheckBox cbUseGoogleDrive;
        private System.Windows.Forms.Label lblStatusLabel;
        private System.Windows.Forms.Label lblGoogleStatus;
        private System.Windows.Forms.Button btnBrowseGoogleKeyFile;
        private System.Windows.Forms.TextBox txtGoogleKeyFile;
        private System.Windows.Forms.Label lblKeyFileLabel;
        private System.Windows.Forms.Button btnTestGoogleConnection;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnCheckBackgroundImages;
        private System.Windows.Forms.Button btnDownloadBackgroundImages;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblBackgroundImagesStatus;
        private System.Windows.Forms.LinkLabel lnkCancelTask;
    }
}