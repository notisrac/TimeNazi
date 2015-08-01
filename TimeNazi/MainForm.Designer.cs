namespace TimeNazi
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.pnlMainContent = new System.Windows.Forms.Panel();
            this.lblWorking = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMute = new System.Windows.Forms.Button();
            this.btnRemoveBackdrop = new System.Windows.Forms.Button();
            this.lblRealClock = new System.Windows.Forms.Label();
            this.btnRefreshImage = new System.Windows.Forms.Button();
            this.pbPictureBox = new System.Windows.Forms.PictureBox();
            this.btnSnooze = new System.Windows.Forms.Button();
            this.lblClock = new System.Windows.Forms.Label();
            this.btnStartWorkPeriod = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tVisualsTimer = new System.Windows.Forms.Timer(this.components);
            this.tRealClockTimer = new System.Windows.Forms.Timer(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlMainContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(308, 514);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSettings.Location = new System.Drawing.Point(3, 514);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // pnlMainContent
            // 
            this.pnlMainContent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMainContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMainContent.Controls.Add(this.lblWorking);
            this.pnlMainContent.Controls.Add(this.pictureBox1);
            this.pnlMainContent.Controls.Add(this.btnMute);
            this.pnlMainContent.Controls.Add(this.btnRemoveBackdrop);
            this.pnlMainContent.Controls.Add(this.lblRealClock);
            this.pnlMainContent.Controls.Add(this.btnRefreshImage);
            this.pnlMainContent.Controls.Add(this.pbPictureBox);
            this.pnlMainContent.Controls.Add(this.btnSnooze);
            this.pnlMainContent.Controls.Add(this.lblClock);
            this.pnlMainContent.Controls.Add(this.btnStartWorkPeriod);
            this.pnlMainContent.Controls.Add(this.lblTitle);
            this.pnlMainContent.Controls.Add(this.btnExit);
            this.pnlMainContent.Controls.Add(this.btnSettings);
            this.pnlMainContent.Location = new System.Drawing.Point(123, 40);
            this.pnlMainContent.Name = "pnlMainContent";
            this.pnlMainContent.Size = new System.Drawing.Size(388, 542);
            this.pnlMainContent.TabIndex = 4;
            this.pnlMainContent.SizeChanged += new System.EventHandler(this.pnlMainContent_SizeChanged);
            // 
            // lblWorking
            // 
            this.lblWorking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblWorking.AutoSize = true;
            this.lblWorking.Location = new System.Drawing.Point(165, 523);
            this.lblWorking.Name = "lblWorking";
            this.lblWorking.Size = new System.Drawing.Size(56, 13);
            this.lblWorking.TabIndex = 12;
            this.lblWorking.Text = "Working...";
            this.lblWorking.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox1.Image = global::TimeNazi.Properties.Resources.clock_161;
            this.pictureBox1.Location = new System.Drawing.Point(134, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(19, 20);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // btnMute
            // 
            this.btnMute.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMute.Image = global::TimeNazi.Properties.Resources.mute;
            this.btnMute.Location = new System.Drawing.Point(33, 3);
            this.btnMute.Name = "btnMute";
            this.btnMute.Size = new System.Drawing.Size(25, 23);
            this.btnMute.TabIndex = 11;
            this.toolTip1.SetToolTip(this.btnMute, "(Un)Mute master volume");
            this.btnMute.UseVisualStyleBackColor = true;
            this.btnMute.Click += new System.EventHandler(this.btnMute_Click);
            // 
            // btnRemoveBackdrop
            // 
            this.btnRemoveBackdrop.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnRemoveBackdrop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveBackdrop.Image = global::TimeNazi.Properties.Resources.no_icon;
            this.btnRemoveBackdrop.Location = new System.Drawing.Point(4, 3);
            this.btnRemoveBackdrop.Name = "btnRemoveBackdrop";
            this.btnRemoveBackdrop.Size = new System.Drawing.Size(25, 23);
            this.btnRemoveBackdrop.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btnRemoveBackdrop, "Remove backdrop image");
            this.btnRemoveBackdrop.UseVisualStyleBackColor = true;
            this.btnRemoveBackdrop.Click += new System.EventHandler(this.btnRemoveBackdrop_Click);
            // 
            // lblRealClock
            // 
            this.lblRealClock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRealClock.AutoSize = true;
            this.lblRealClock.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblRealClock.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblRealClock.Location = new System.Drawing.Point(331, 2);
            this.lblRealClock.Name = "lblRealClock";
            this.lblRealClock.Size = new System.Drawing.Size(52, 22);
            this.lblRealClock.TabIndex = 9;
            this.lblRealClock.Text = "13:37";
            this.toolTip1.SetToolTip(this.lblRealClock, "Current time");
            // 
            // btnRefreshImage
            // 
            this.btnRefreshImage.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnRefreshImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshImage.Image = global::TimeNazi.Properties.Resources.refresh;
            this.btnRefreshImage.Location = new System.Drawing.Point(62, 2);
            this.btnRefreshImage.Name = "btnRefreshImage";
            this.btnRefreshImage.Size = new System.Drawing.Size(25, 25);
            this.btnRefreshImage.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnRefreshImage, "Change image (random!)");
            this.btnRefreshImage.UseVisualStyleBackColor = true;
            this.btnRefreshImage.Click += new System.EventHandler(this.btnRefreshImage_Click);
            // 
            // pbPictureBox
            // 
            this.pbPictureBox.BackColor = System.Drawing.SystemColors.Control;
            this.pbPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPictureBox.Location = new System.Drawing.Point(11, 35);
            this.pbPictureBox.Name = "pbPictureBox";
            this.pbPictureBox.Size = new System.Drawing.Size(365, 388);
            this.pbPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPictureBox.TabIndex = 7;
            this.pbPictureBox.TabStop = false;
            this.pbPictureBox.WaitOnLoad = true;
            this.pbPictureBox.VisibleChanged += new System.EventHandler(this.pictureBox1_VisibleChanged);
            // 
            // btnSnooze
            // 
            this.btnSnooze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSnooze.Location = new System.Drawing.Point(170, 477);
            this.btnSnooze.Name = "btnSnooze";
            this.btnSnooze.Size = new System.Drawing.Size(133, 43);
            this.btnSnooze.TabIndex = 4;
            this.btnSnooze.Text = "Snooze";
            this.btnSnooze.UseVisualStyleBackColor = true;
            this.btnSnooze.Click += new System.EventHandler(this.btnSnooze_Click);
            // 
            // lblClock
            // 
            this.lblClock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblClock.AutoSize = true;
            this.lblClock.Font = new System.Drawing.Font("Consolas", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblClock.Location = new System.Drawing.Point(120, 418);
            this.lblClock.Name = "lblClock";
            this.lblClock.Size = new System.Drawing.Size(154, 56);
            this.lblClock.TabIndex = 6;
            this.lblClock.Text = "00:00";
            this.toolTip1.SetToolTip(this.lblClock, "Time remaining");
            // 
            // btnStartWorkPeriod
            // 
            this.btnStartWorkPeriod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartWorkPeriod.Location = new System.Drawing.Point(84, 477);
            this.btnStartWorkPeriod.Name = "btnStartWorkPeriod";
            this.btnStartWorkPeriod.Size = new System.Drawing.Size(133, 43);
            this.btnStartWorkPeriod.TabIndex = 5;
            this.btnStartWorkPeriod.Text = "Start working";
            this.btnStartWorkPeriod.UseVisualStyleBackColor = true;
            this.btnStartWorkPeriod.Click += new System.EventHandler(this.btnStartWorkPeriod_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Trebuchet MS", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(386, 29);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "TimeNazi";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tVisualsTimer
            // 
            this.tVisualsTimer.Interval = 1000;
            this.tVisualsTimer.Tick += new System.EventHandler(this.tVisualsTimer_Tick);
            // 
            // tRealClockTimer
            // 
            this.tRealClockTimer.Enabled = true;
            this.tRealClockTimer.Interval = 1000;
            this.tRealClockTimer.Tick += new System.EventHandler(this.tRealClockTimer_Tick);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 796);
            this.ControlBox = false;
            this.Controls.Add(this.pnlMainContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "TimeNazi";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.pnlMainContent.ResumeLayout(false);
            this.pnlMainContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Panel pnlMainContent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnStartWorkPeriod;
        private System.Windows.Forms.Button btnSnooze;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.PictureBox pbPictureBox;
        private System.Windows.Forms.Button btnRefreshImage;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer tVisualsTimer;
        private System.Windows.Forms.Label lblRealClock;
        private System.Windows.Forms.Timer tRealClockTimer;
        private System.Windows.Forms.Button btnRemoveBackdrop;
        private System.Windows.Forms.Button btnMute;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblWorking;
    }
}

