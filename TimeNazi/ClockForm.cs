using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using nUtils.WinForms;

namespace TimeNazi
{
    public class ClockForm : Form
    {
        private const int WS_EX_TOPMOST = 0x8000000;
        private const int WS_CHILD = 0x40000000;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private PictureBox pictureBox1;
        private Timer tVisualsTimer;
        private System.ComponentModel.IContainer components;
        private const int WM_NCLBUTTONDOWN = 0xA1;
        [DllImport("User32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private Label lblClockFace;
        private bool _bColonVisible = true;
        private string _sClockValue = "00:00";

        public bool ClockEnabled
        {
            get 
            { 
                return tVisualsTimer.Enabled;
            }
            set
            {
                tVisualsTimer.Enabled = value;
                _toggleColon(true);
            }
        }


        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;
            }
        }


        public ClockForm()
        {
            this.InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Size = new System.Drawing.Size(93, 30);
            //this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height - 50;
            //this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width - 10;
            this.TopMost = true;
            this.Opacity = (float)Properties.Settings.Default.ClockOpacity / 100;
            _toggleColon(true);
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        //make sure Top Most property on form is set to false
        //        //otherwise this doesn't work
        //        CreateParams cp = base.CreateParams;
        //        //cp.Style |= WS_CHILD;
        //        cp.ExStyle |= WS_EX_TOPMOST;
        //        return cp;
        //    }
        //}

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClockForm));
            this.lblClockFace = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tVisualsTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClockFace
            // 
            this.lblClockFace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblClockFace.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblClockFace.Location = new System.Drawing.Point(0, 0);
            this.lblClockFace.Name = "lblClockFace";
            this.lblClockFace.Size = new System.Drawing.Size(96, 33);
            this.lblClockFace.TabIndex = 0;
            this.lblClockFace.Text = "00:00";
            this.lblClockFace.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblClockFace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblClockFace_MouseDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(21, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblClockFace_MouseDown);
            // 
            // tVisualsTimer
            // 
            this.tVisualsTimer.Interval = 1000;
            this.tVisualsTimer.Tick += new System.EventHandler(this.tVisualsTimer_Tick);
            // 
            // ClockForm
            // 
            this.ClientSize = new System.Drawing.Size(96, 33);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblClockFace);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClockForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        private void lblClockFace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }

        public void SetClock(TimeSpan time)
        {
            int iHours = time.Hours;
            int iMinutes = time.Minutes;
            if (time.Seconds > 0)
            {
                iMinutes += 1;
            }

            string sNewClockValue = string.Format("{0}:{1}", iHours.ToString("00"), iMinutes.ToString("00"));

            if (0 != string.Compare(sNewClockValue, _sClockValue))
            {
                _sClockValue = sNewClockValue;
                //if (lblClockFace.InvokeRequired)
                //{
                //    lblClockFace.Invoke(new MethodInvoker(delegate { lblClockFace.Text = sClockText; }));
                //}
                //else
                //{
                //    lblClockFace.Text = sClockText;
                //}
                lblClockFace.InvokeMember("Text", _sClockValue);
            }
        }

        [DebuggerStepThrough]
        private void tVisualsTimer_Tick(object sender, EventArgs e)
        {
            //Trace.TraceInformation("[ClockForm] timer elapsed");
            _toggleColon(!_bColonVisible);
        }

        private void _toggleColon(bool visible)
        {
            string sClockText = _sClockValue;
            if (visible)
            {
                lblClockFace.Text = sClockText.Replace(" ", ":");
            }
            else
            {
                lblClockFace.Text = sClockText.Replace(":", " ");
            }
            lblClockFace.Invalidate();
            //lblClockFace.Refresh();
            //lblClockFace.Update();
            _bColonVisible = visible;
            //Trace.TraceInformation("[ClockForm] label changed lblClockFace.Text={0}, visible={1}, _bColonVisible={2}", lblClockFace.Text, visible, _bColonVisible);
        }
    }
}
