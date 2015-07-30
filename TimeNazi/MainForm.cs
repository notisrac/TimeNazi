using nUtils.Gfx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using nUtils.WinForms;
using CoreAudio;
using NLog;

namespace TimeNazi
{
    public partial class MainForm : OverlayForm //Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

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

        //private Bitmap _backgroundImage = null;

        public delegate void SettingsClickedEventHandler(object sender, EventArgs e);
        public event SettingsClickedEventHandler SettingsClicked;
        public delegate void StartClickedEventHandler(object sender, EventArgs e);
        public event StartClickedEventHandler StartClicked;
        public delegate void SnoozeClickedEventHandler(object sender, EventArgs e);
        public event SnoozeClickedEventHandler SnoozeClicked;

        private static readonly InterceptKeys.LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        private Random _rnd = new Random();

        private bool _bColonVisible = true;
        private string _sClockValue = "00:00";
        private Size _sizMainPanelSize = new Size(0, 0);
        private MMDevice _mmdAudioDevice;

        List<OverlayForm> _lofOverlayForms = new List<OverlayForm>();

        public MainForm()
        {
            logger.Debug("MainForm()");
            InitializeComponent();
            this.Icon = Properties.Resources.clock;
            // create the refresh image button's completely flat appearance
            //btnRefreshImage.FlatStyle = FlatStyle.Flat;
            //btnRefreshImage.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //Transparent
            //btnRefreshImage.FlatAppearance.BorderSize = 0;
            //btnRefreshImage.FlatAppearance.MouseOverBackColor = pnlMainContent.BackColor;
            //btnRefreshImage.FlatAppearance.MouseDownBackColor = pnlMainContent.BackColor;

            _sizMainPanelSize = pnlMainContent.Size;
            _sizMainPanelSize.Height -= pbPictureBox.Height;
            _updateRealClock();

            try
            {
                logger.Debug("setting up audio device control");
                // set up the audio device control
                MMDeviceEnumerator mmdEnumerator = new MMDeviceEnumerator();
                _mmdAudioDevice = mmdEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to set up the audio device: {0}", ex.Message);
            }
#if !DEBUG
            btnExit.Visible = false;
#endif
        }

        protected override void OnShown(EventArgs e)
        {
            logger.Debug("OnShown()");
            try
            {
                // try to set the keyboard hook
                _hookID = InterceptKeys.SetHook(_proc);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to set the keyboard hook: {0}", ex.Message);
                DetachKeyboardHook();
            }

            base.OnShown(e);
            
            this.CenterToScreen();
            //// make it fullscreen
            //// always on top
            //this.TopMost = true;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            _displayOverlayForms();

            _redrawMainPanel();
            this.Focus();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            logger.Debug("OnClosing()");
            _closeOverlayForms();
            base.OnClosing(e);
        }

        private void _displayOverlayForms()
        {
            logger.Debug("Opening overlay forms");
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.Equals(Screen.FromControl(this)))
                { // skip the screen the main window is on
                    logger.Trace("Skipping the window of the mainform");
                    continue;
                }
                logger.Trace("Overlay form at: {0}", screen.Bounds.Location);
                OverlayForm ofForm = new OverlayForm();
                ofForm.Location = screen.Bounds.Location;
                ofForm.Icon = Properties.Resources.clock;
                ofForm.Initialize();
                ofForm.Show();
                _lofOverlayForms.Add(ofForm);
            }
        }

        private void _closeOverlayForms()
        {
            logger.Debug("Closing overlay forms");
            foreach (var overlayForm in _lofOverlayForms)
            {
                if (null != overlayForm)
                {
                    overlayForm.Close();
                }
            }
            _lofOverlayForms = new List<OverlayForm>();
        }

        private void _redrawMainPanel()
        {
            logger.Debug("_redrawMainPanel()");
            if (!pbPictureBox.Visible)
            {
                //if (pnlMainContent.Height > pbPictureBox.Height)
                //{
                //    pnlMainContent.Height -= pbPictureBox.Height;
                //}
                pnlMainContent.Size = _sizMainPanelSize;
            }
            else
            {
                pnlMainContent.Height = _sizMainPanelSize.Height + pbPictureBox.Height;
            }
            //pnlMainContent.PerformLayout();
            pnlMainContent.Width = pbPictureBox.Width + 20;
            pbPictureBox.Left = pnlMainContent.Width / 2 - pbPictureBox.Width / 2;
            // center the main panel on the form
            pnlMainContent.Top = this.Height / 2 - pnlMainContent.Height / 2;
            pnlMainContent.Left = this.Width / 2 - pnlMainContent.Width / 2;
            // center the start and snooze buttons on the main panel
            btnStartWorkPeriod.Left = pnlMainContent.Width / 2 - btnStartWorkPeriod.Width / 2;
            btnSnooze.Left = pnlMainContent.Width / 2 - btnSnooze.Width / 2;
            // center the clock on the main panel
            lblClock.Left = pnlMainContent.Width / 2 - lblClock.Width / 2;
            // stick the image refresh button to the bottom of the image box
            //btnRefreshImage.Top = pbPictureBox.Top + pbPictureBox.Height + 1;
        }

        //public void Initialize()
        //{
        //    logger.Debug("Initialize()");
        //    _setBackground();
        //}

        //private void _setBackground()
        //{
        //    logger.Debug("_setBackground()");
        //    // set the background color to black
        //    this.BackColor = Color.Black;
        //    // create a screenshot of the background
        //    Rectangle bounds = Screen.GetBounds(Point.Empty);
        //    _backgroundImage = new Bitmap(bounds.Width, bounds.Height);
        //    using (Graphics g = Graphics.FromImage(_backgroundImage))
        //    {
        //        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
        //    }
        //    //_backgroundImage.Save(@"c:\Users\Noti\Dropbox\Projects\TimeNazi\TimeNazi\!tmp\" + DateTime.Now.Ticks + ".jpg", ImageFormat.Jpeg);
        //    // set it as the background, but make it grayscale first
        //    this.BackgroundImage = GfxUtils.MakeGrayscale3(_backgroundImage);
        //}

        private void _setBackgroundToBlack()
        {
            logger.Debug("_setBackgroundToBlack()");
            // set the background color to black
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            this.BackgroundImage = new Bitmap(bounds.Width, bounds.Height);
        }

        private void _setAllBackdropsToBlack()
        {
            logger.Debug("_setBackgroundToBlack()");
            foreach (var overlayForm in _lofOverlayForms)
            {
                if (null != overlayForm)
                {
                    overlayForm.SetBackgroundToBlack();
                }
            }
        }

        public void SetClock(TimeSpan time)
        {
            logger.Trace(string.Format("SetClock({0})", time.ToString()));
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
                lblClock.InvokeMember("Text", _sClockValue);
            }
        }

        public void ScenarioStart(TimeSpan time)
        {
            logger.Debug(string.Format("ScenarioStart({0})", time.ToString()));
            pnlMainContent.Height -= pbPictureBox.Height;
            pbPictureBox.Visible = false;
            btnRefreshImage.Visible = false;
            btnSnooze.Visible = false;
            btnStartWorkPeriod.Visible = true;
            SetClock(time);
            // temp!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //pbPictureBox.Visible = true;
            //btnRefreshImage.Visible = true;
            //_loadImage();
            //_redrawMainPanel();
        }

        public void ScenarioRest(TimeSpan time, bool snoozeEnabled)
        {
            logger.Debug(string.Format("ScenarioRest({0}, {1})", time.ToString(), snoozeEnabled));
            _loadImage();
            //pbPictureBox.InvokeMember("Visible", true);
            //btnSnooze.InvokeMember("Visible", true);
            //btnSnooze.InvokeMember("Enabled", snoozeEnabled);
            //btnStartWorkPeriod.InvokeMember("Visible", false);
            pbPictureBox.Visible = true;
            btnRefreshImage.Visible = true;
            btnSnooze.Visible = true;
            btnSnooze.Enabled = snoozeEnabled;
            btnStartWorkPeriod.Visible = false;
            btnSettings.Visible = false;
            //btnExit.Visible = false;
            SetClock(time);
            _redrawMainPanel();
        }

        public void ToggleWorking(bool isWorking)
        {
            this.lblWorking.Visible = isWorking;
            this.pnlMainContent.Enabled = !isWorking;
        }

        private void _loadImage()
        {
            logger.Debug("_loadImage()");
            Bitmap bmpImage = null;
            string[] saFiles = null;
            // look for the images in the bgimages dir
            string sBGImageLocation = Path.Combine(Constants.ApplicationDataDir, Constants.FILESYSTEM_BACKGROUNDIMAGES);

            if (Directory.Exists(sBGImageLocation))
            {
                saFiles = Directory.GetFiles(sBGImageLocation, "*.jpg");
            }

            if (null != saFiles && saFiles.Length > 0)
            { // get a random file from the cache/image location
                logger.Debug("_loadImage() selecting a file from the available bg images");
                int iBGRnd = _rnd.Next(saFiles.Length);
                bmpImage = (Bitmap)Bitmap.FromFile(saFiles[iBGRnd]);
            }
            else
            {
                logger.Debug("_loadImage() no bg images");
                // if no image is found in the cache/dir
                // display one of the nobg images
                Bitmap[] baNoBGList = new Bitmap[] { Properties.Resources.nobg_otter, Properties.Resources.nobg_patrick, Properties.Resources.nobg_yuno };
                int iNoBGRnd = _rnd.Next(baNoBGList.Length);
                //pbPictureBox.Image = baNoBGList[iNoBGRnd];
                bmpImage = baNoBGList[iNoBGRnd];
                baNoBGList = null;
            }
            if (null != bmpImage)
            { // display the image
                pbPictureBox.InvokeMember("Image", bmpImage);
            }
        }

        private void _updateRealClock()
        {
            lblRealClock.Text = DateTime.Now.ToString("HH:mm");
        }

        private void _toggleColon(bool visible)
        {
            string sClockText = _sClockValue;
            if (visible)
            {
                lblClock.Text = sClockText.Replace(" ", ":");
            }
            else
            {
                lblClock.Text = sClockText.Replace(":", " ");
            }
            lblClock.Invalidate();
            //lblClockFace.Refresh();
            //lblClockFace.Update();
            _bColonVisible = visible;
            //Trace.TraceInformation("[ClockForm] label changed lblClockFace.Text={0}, visible={1}, _bColonVisible={2}", lblClockFace.Text, visible, _bColonVisible);
        }

        #region keyboard_hook
        /// <summary>
        /// Detach the keyboard hook; call during shutdown to prevent calls as we unload
        /// </summary>
        private void DetachKeyboardHook()
        {
            if (_hookID != IntPtr.Zero)
                InterceptKeys.UnhookWindowsHookEx(_hookID);
        }

        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                bool alt = (Control.ModifierKeys & Keys.Alt) != 0;
                bool control = (Control.ModifierKeys & Keys.Control) != 0;
                bool shift = (Control.ModifierKeys & Keys.Shift) != 0;


                int vkCode = Marshal.ReadInt32(lParam);
                Keys key = (Keys)vkCode;

                if (alt && key == Keys.F4)
                {
                    //Application.Current.Shutdown();
                    //this.Close();
                    return (IntPtr)1; // Handled.
                }

                if (alt && control && shift && key == Keys.F12)
                {
                    logger.Debug("Hotkey exit!");
                    Application.Exit();
                    return (IntPtr)1; // Handled.
                }

                if (!AllowKeyboardInput(alt, control, shift, key))
                {
                    return (IntPtr)1; // Handled.
                }
            }

            return InterceptKeys.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        /// <summary>Determines whether the specified keyboard input should be allowed to be processed by the system.</summary>
        /// <remarks>Helps block unwanted keys and key combinations that could exit the app, make system changes, etc.</remarks>
        public static bool AllowKeyboardInput(bool alt, bool control, bool shift, Keys key)
        {
            logger.Trace("AllowKeyboardInput() alt:{0}, control:{1}, shift:{2}, key:{3}", alt, control, shift, key);
            // Disallow various special keys.
            if (key <= Keys.Back || key == Keys.None ||
                key == Keys.Menu || key == Keys.Pause ||
                key == Keys.Help)
            {
                return false;
            }

            // Disallow ranges of special keys.
            // Currently leaves volume controls enabled; consider if this makes sense.
            // Disables non-existing Keys up to 65534, to err on the side of caution for future keyboard expansion.
            if ((key >= Keys.LWin && key <= Keys.Sleep) ||
                (key >= Keys.KanaMode && key <= Keys.HanjaMode) ||
                (key >= Keys.IMEConvert && key <= Keys.IMEModeChange) ||
                (key >= Keys.BrowserBack && key <= Keys.BrowserHome) ||
                (key >= Keys.MediaNextTrack && key <= Keys.LaunchApplication2) ||
                (key >= Keys.ProcessKey && key <= (Keys)65534))
            {
                return false;
            }

            // Disallow specific key combinations. (These component keys would be OK on their own.)
            if ((alt && key == Keys.Tab) ||
                (alt && key == Keys.Space) ||
                (control && key == Keys.Escape) ||
                (control && key == Keys.Escape && shift))
            {
                return false;
            }

            // Allow anything else (like letters, numbers, spacebar, braces, and so on).
            return true;
        }
        #endregion

        #region eventhandlers
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetachKeyboardHook();
        }

        private void btnStartWorkPeriod_Click(object sender, EventArgs e)
        {
            if (null != StartClicked)
            {
                StartClicked(this, EventArgs.Empty);
            }
            //this.Close();
        }

        private void pictureBox1_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void pnlMainContent_SizeChanged(object sender, EventArgs e)
        {
            //_redrawMainPanel(false);
        }

        private void btnSnooze_Click(object sender, EventArgs e)
        {
            if (null != SnoozeClicked)
            {
                SnoozeClicked(this, EventArgs.Empty);
            }
            //this.Close();
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                DetachKeyboardHook();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (null != SettingsClicked)
            {
                SettingsClicked(this, EventArgs.Empty);
            }
        }

        private void btnRefreshImage_Click(object sender, EventArgs e)
        {
            _loadImage();
            _redrawMainPanel();
        }

        private void tVisualsTimer_Tick(object sender, EventArgs e)
        {
            _toggleColon(!_bColonVisible);
        }

        private void tRealClockTimer_Tick(object sender, EventArgs e)
        {
            _updateRealClock();
        }
        #endregion

        private void btnRemoveBackdrop_Click(object sender, EventArgs e)
        {
            _setBackgroundToBlack();
            _setAllBackdropsToBlack();
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            try
            {
                _mmdAudioDevice.AudioEndpointVolume.Mute = !_mmdAudioDevice.AudioEndpointVolume.Mute;
                logger.Debug(string.Format("Master volume set to: {0}", _mmdAudioDevice.AudioEndpointVolume.Mute ? "unmute" : "mute"));
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to mute/unmute audio: {0}", ex.Message);
            }
        }
    }
}
