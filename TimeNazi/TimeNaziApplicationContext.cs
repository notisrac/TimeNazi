using Microsoft.Win32;
using NLog;
using nUtils.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// http://www.codeproject.com/Tips/627796/Doing-a-NotifyIcon-program-the-right-way
// http://stackoverflow.com/questions/725627/accessing-google-spreadsheets-with-c-sharp-using-google-data-api
// https://developers.google.com/google-apps/spreadsheets/


/*
 * DONE the clock form should have a show method like _showMainForm - show the clock form and set the clock on show
 * NOPE mainform: remove all resize code - the autosize and well placed anchors should do the job
 * DONE implement the snooze button
 * DONE grumpy cat meme: want to work some more? NO!
 * DONE start the timer in the rest scenario
 * DONE implement the blinking semicolon on the main form
 * DONE mainform: remove the settings button
 * DONE mainform: remove the exit button
 * DONE mainform: add a special key combo to exit the app/close the rest screen - and disable/exit the app maybe (ALT+CTRL+SHIFT+F12)
 * DONE add an enable/disable menu item to the tray icon to enable/disable the app
 * DONE add a popup bubble to the tray icon indicating the start of the last 60/30/10 seconds (only in work mode) - how fast does a notification bubble dissapear?
 * DONE mainform: add a next random background image button
 * DONE add a clock that shows the current time
 * DONE add a start with windows checkbox to the settings http://stackoverflow.com/questions/7427354/program-start-with-windows-c-sharp (for current user only. global needs elevated privilages, and that needs a separate app - PITA!!!!)
 * DONE add a test connection button to the database settings
 * DONE fix the "show clock" check menu item
 * DONE update the clocks based on the orchestrator ticks
 * DONE handle sleep mode: http://stackoverflow.com/a/4574888 - do not forget to detach from the event when the app closes
 * DONE add a mute button to mute everything - since you cannot access anything outside the app when the rest screen is active - http://whenimbored.xfx.net/2011/01/core-audio-for-net/
 * DONE what to do when there is no internet connection? ge. cannot download the config
 * DONE download the bgimages from the net (github pages), add a download button to the settings screen
 * DONE add button, to black out the background screen (in case something inappropriate is on it :))
 * DONE add activity logging
 * DONE handle workstation lock - http://stackoverflow.com/questions/44980/how-can-i-programmatically-determine-if-my-workstation-is-locked
 * DONE add an installer
 * DONE handle multiple displays
 * DONE store the position of the clock (settings file)
 * TODO Implement idle timeout
 */


namespace TimeNazi
{
    public class TimeNaziApplicationContext : ApplicationContext, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private NotifyIcon _niTrayicon;
        private ContextMenuStrip _cmsTrayIconContextMenu;
        private ToolStripMenuItem _tsmiMenuItemExit;
        private ToolStripMenuItem _tsmiMenuItemSettings;
        private ToolStripSeparator _tssSeparator;
        private ToolStripMenuItem _tsmiMenuItemShowClock;
        private ToolStripMenuItem _tsmiMenuItemEnable;
        private ConfigurationManager _cmConfigManager;
        private GoogleApiWrapper _gawGoogleWrapper = new GoogleApiWrapper();
        private ActivityLogManager _almActivityLogManager = new ActivityLogManager();
        private long _lSOElapsedTime = 0;
        private DateTime _dtAppStart = DateTime.Now;
        private bool _bIsSessionLocked = false;

        // Used to check if the screen saver is running
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SystemParametersInfo(uint uAction, uint uParam, ref bool lpvParam, int fWinIni);


        private ClockForm _cfClockForm;
        public ClockForm clockForm
        {
            get
            {
                if (null == _cfClockForm || _cfClockForm.IsDisposed || _cfClockForm.Disposing)
                {
                    logger.Debug("new ClockForm() instantiated");
                    _cfClockForm = new ClockForm();
                    _cfClockForm.Shown += _cfClockForm_Shown;
                    _cfClockForm.Location = _cmConfigManager.Config.ClockFormPosition;
                    _cfClockForm.FormClosing += _cfClockForm_FormClosing;
                }
                return _cfClockForm;
            }
        }

        private SettingsForm _sfSettingsForm;
        public SettingsForm settingsForm
        {
            get
            {
                if (null == _sfSettingsForm || _sfSettingsForm.IsDisposed || _sfSettingsForm.Disposing)
                {
                    logger.Debug("new SettingsForm() instantiated");
                    _sfSettingsForm = new SettingsForm();
                }
                return _sfSettingsForm;
            }
        }

        private MainForm _mfMainForm;
        public MainForm mainForm
        {
            get
            {
                if (null == _mfMainForm || _mfMainForm.IsDisposed || _mfMainForm.Disposing)
                {
                    logger.Debug("new MainForm() instantiated");
                    _mfMainForm = new MainForm();
                    mainForm.SettingsClicked += _mfMainForm_SettingsClicked;
                    mainForm.StartClicked += _mfMainForm_StartClicked;
                    mainForm.SnoozeClicked += _mfMainForm_SnoozeClicked;
                }
                return _mfMainForm;
            }
            set
            {
                _mfMainForm = value;
            }
        }
        private ScenarioOrchestrator _soOrchestrator;


        public TimeNaziApplicationContext()
        {
            logger.Debug("TimeNaziApplicationContext() ctor");
            InitializeComponent();
            _initConfig();
            InitializeComponent_needsConfig();


            // handle the exit event
            Application.ApplicationExit += Application_ApplicationExit;
            // handle the opacity change of the clock on the settings page
            settingsForm.ClockOpacityChanged += _sfSettingsForm_ClockOpacityChanged;

            logger.Debug("Setting up orchestrator");
            _soOrchestrator = new ScenarioOrchestrator();
            _loadConfig();
            _soOrchestrator.RestElapsed += _soOrchestrator_RestElapsed;
            _soOrchestrator.SnoozeElapsed += _soOrchestrator_SnoozeElapsed;
            _soOrchestrator.WorkElapsed += _soOrchestrator_WorkElapsed;
            _soOrchestrator.Tick += _soOrchestrator_Tick;
            _soOrchestrator.MinuteWarning += _soOrchestrator_MinuteWarning;
            _soOrchestrator.Initialize();

            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            logger.Debug("Application start: scenario start");
            _scenarioStartup();
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Suspend:
                    logger.Debug("PowerModes.Suspend");
                    if (_bIsSessionLocked)
                    {
                        break;
                    }
                    logger.Debug("Pause because of PowerModes.Suspend event");
                    _soOrchestrator.Pause();
                    break;
                case PowerModes.Resume:
                    logger.Debug("PowerModes.Resume");
                    if (_bIsSessionLocked)
                    {
                        break;
                    }
                    logger.Debug("Resume because of PowerModes.Resume event");
                    _soOrchestrator.Resume();
                    break;
                default:
                    break;
            }
        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    _bIsSessionLocked = true;
                    logger.Debug("Pause because of SessionSwitchReason.SessionLock event");
                    _soOrchestrator.Pause();
                    break;
                case SessionSwitchReason.SessionUnlock:
                    _bIsSessionLocked = false;
                    logger.Debug("Resume because of SessionSwitchReason.SessionUnlock event");
                    _soOrchestrator.Resume();
                    break;
                default:
                    break;
            }
        }

        // Check if the screensaver is busy running.
        public bool IsScreensaverRunning()
        {
            const int SPI_GETSCREENSAVERRUNNING = 114;
            bool isRunning = false;

            if (!SystemParametersInfo(SPI_GETSCREENSAVERRUNNING, 0, ref isRunning, 0))
            {
                // Could not detect screen saver status...
                return false;
            }

            if (isRunning)
            {
                // Screen saver is ON.
                return true;
            }

            // Screen saver is OFF.
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            logger.Debug("disposing...");
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            SystemEvents.PowerModeChanged -= SystemEvents_PowerModeChanged;

            switch (_soOrchestrator.CurrentState)
            {
                case States.Working:
                    _logActivity(ActivityType.WorkEnd, _lSOElapsedTime);
                    break;
                case States.Snooze:
                    _logActivity(ActivityType.SnoozeEnd, _lSOElapsedTime);
                    break;
                case States.Resting:
                    _logActivity(ActivityType.RestEnd, _lSOElapsedTime);
                    break;
            }

            _logActivity(ActivityType.AppStop, (long)(DateTime.Now - _dtAppStart).TotalSeconds);
            if (null != _gawGoogleWrapper)
            {
                _gawGoogleWrapper.Dispose();
            }
            base.Dispose(disposing);
        }

        private void _initConfig()
        {
            logger.Debug("initializing the config manager");
            try
            {
                // init the config man
                _cmConfigManager = new ConfigurationManager();
                // load the local config
                _cmConfigManager.LoadLocal();
                // check if a db should be used, or not
                if (_cmConfigManager.Config.UseDataBase && !string.IsNullOrWhiteSpace(_cmConfigManager.Config.DatabaseKeyFile))
                {
                    logger.Debug("config manager using the database");
                    // check for internet connection: http://stackoverflow.com/questions/2031824/what-is-the-best-way-to-check-for-internet-connectivity-using-net
                    if (NetworkUtils.CheckInternetConnection())
                    {
                        logger.Debug("initialising the google api wrapper");
                        // init the google wrapper
                        //_gawGoogleWrapper = new GoogleApiWrapper();
                        _gawGoogleWrapper.LoadAuthParams(_cmConfigManager.Config.DatabaseKeyFile);
                        _gawGoogleWrapper.TokenRefreshed += _gawGoogleWrapper_TokenRefreshed;
                        if (!_gawGoogleWrapper.NeedsAuthorization)
                        { // it needs to be authorized before use
                            logger.Debug("initialising the database wrapper");
                            // init the database handler
                            DatabaseWrapper dbWrapper = new DatabaseWrapper(_gawGoogleWrapper.AuthParams);
                            // try initing the config table
                            if (dbWrapper.InitializeConfigTable() && dbWrapper.InitializeActivityLogTable())
                            {
                                // tell the config man to use the database
                                _cmConfigManager.Database = dbWrapper;
                                // reload the config
                                _cmConfigManager.Load();
                                // tell the activity log man to use the database
                                _almActivityLogManager.Database = dbWrapper;
                            }
                        }
                        else
                        {
                            logger.Warn("App not authorized: {0}", _cmConfigManager.Config.DatabaseKeyFile);
                            _niTrayicon.ShowBalloonTip(15000, "Warning", "The app needs to be authorized before using the database! Go to settings!", ToolTipIcon.Warning);
                        }
                    }
                    else
                    {
                        logger.Warn("UseDatabase = true, but no internet connection");
                        _niTrayicon.ShowBalloonTip(15000, "Warning", "The app is configured to use the database, but no internet connection available. Falling back to offline mode.", ToolTipIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while loading the config: {0}", ex.Message);
                MessageBox.Show("Error loading config! Using default values.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _logActivity(ActivityType activityType, long elapsedTime)
        {
            logger.Debug("Storing new activity: {0}", activityType);
            _almActivityLogManager.StoreActivity(new ActivityLogEntry() { LogDate = DateTime.Now, UserName = Environment.UserName, Source = Environment.MachineName, Activity = activityType, ElapsedTime = elapsedTime });
        }

        void _gawGoogleWrapper_TokenRefreshed(object sender, EventArgs e)
        {
            logger.Debug("adding renewed google token to the config manager:");
            _cmConfigManager.Database.Initialize(((GoogleApiWrapper)sender).AuthParams);
            logger.Debug("and the activity log manager:");
            _almActivityLogManager.Initialize(((GoogleApiWrapper)sender).AuthParams);
        }

        void _soOrchestrator_MinuteWarning(object sender, EventArgs e)
        {
            logger.Debug("minute warning");
            _niTrayicon.ShowBalloonTip(15000, "Minute warning", "One minute remaining!", ToolTipIcon.Info);
        }

        void _soOrchestrator_Tick(object sender, ElapsedEventArgs e)
        {
            _lSOElapsedTime = e.ElapsedTime;
            _setClocks(_soOrchestrator.GetCurrentTimer());
            //if (IsScreensaverRunning())
            //{
            //    if (!_soOrchestrator.IsPaused)
            //    {
            //        logger.Debug("Pause because of IsScreensaverRunning=true event");
            //        _soOrchestrator.Pause();
            //    }
            //}
            //else
            //{
            //    if (_soOrchestrator.IsPaused)
            //    {
            //        logger.Debug("Resume because of IsScreensaverRunning=false event");
            //        _soOrchestrator.Resume();
            //    }
            //}
        }

        private void _loadConfig()
        {
            logger.Debug("loading config into orchestrator");
            _soOrchestrator.NumberOfSnoozes = _cmConfigManager.Config.NumberOfSnoozes;
            _soOrchestrator.PauseTime = _cmConfigManager.Config.PauseTimeout;
            _soOrchestrator.RestTime = _cmConfigManager.Config.RestTime;
            _soOrchestrator.SnoozeTime = _cmConfigManager.Config.SnoozeTime;
            _soOrchestrator.WorkTime = _cmConfigManager.Config.WorkTime;
        }

        private void _scenarioStartup()
        { // display all the things needed for starting the scenarion (and wait for the user to press the start button)
            logger.Debug("scenario: startup");
            _logActivity(ActivityType.AppStart, 0);
            _showMainForm();
            _mfMainForm.ClockEnabled = false;
            _setClocks(_soOrchestrator.GetCurrentTimer());
            clockForm.Show();
            clockForm.ClockEnabled = false;
            mainForm.ScenarioStart(_soOrchestrator.GetCurrentTimer());
        }

        private void _scenarioWorking(/*bool isSnooze*/)
        { // start the timer, and close the main window, so the user can work freely
            logger.Debug("scenario: working");
            _setSideClock(_soOrchestrator.GetCurrentTimer());
            clockForm.Show();
            clockForm.ClockEnabled = true;
            mainForm.Close();
            //mainForm.InvokeMethod("Hide", null);
        }

        private void _scenarioResting()
        { // start the timer, and close the main window, so the user can work freely
            logger.Debug("scenario: resting");
            _setClocks(_soOrchestrator.GetCurrentTimer());
            clockForm.Hide();
            clockForm.ClockEnabled = false;
            _showMainForm();
            _mfMainForm.ClockEnabled = true;
            mainForm.ScenarioRest(_soOrchestrator.GetCurrentTimer(), _soOrchestrator.NumberOfSnoozesRemaining > 0);
        }

        private void _setClocks(TimeSpan setTime)
        {
            _setMainClock(setTime);
            _setSideClock(setTime);
        }

        private void _setMainClock(TimeSpan setTime)
        {
            mainForm.SetClock(setTime);
        }

        private void _setSideClock(TimeSpan setTime)
        {
            clockForm.SetClock(setTime);
        }

        private TimeSpan _getWorkTimeFromSettings()
        {
            return TimeSpan.FromMinutes(_cmConfigManager.Config.WorkTime);
        }

        private void _setClockOpacity(int opacity)
        {
            if (null != clockForm)
            {
                if (opacity < 0)
                {
                    opacity = 0;
                }
                if (opacity > 100)
                {
                    opacity = 100;
                }
                clockForm.Opacity = (float)opacity / 100;
            }
        }

        private void _changeClockVisibility(object sender)
        {
            ToolStripMenuItem senderObject = (ToolStripMenuItem)sender;
            if (senderObject.Checked)
            {
                logger.Debug("clock is now visible");
                clockForm.Show();
                _cmConfigManager.Config.ShowClock = true;
                _cmConfigManager.Save();
                _setClocks(_getWorkTimeFromSettings());
            }
            else
            {
                logger.Debug("clock is now hidden");
                clockForm.Close();
                _cmConfigManager.Config.ShowClock = false;
                _cmConfigManager.Save();
            }
        }

        private void _showSettingsForm(bool isModal)
        {
            logger.Debug("settingsform.show(" + isModal + ")");
            settingsForm.ConfigManager = _cmConfigManager;
            settingsForm.GoogleWrapper = _gawGoogleWrapper;
            if (isModal)
            {
                mainForm.TopMost = false;
                settingsForm.ShowDialog();
                mainForm.TopMost = true;
            }
            else
            {
                settingsForm.Show();
            }
            // need to update the forms too - only if the timers are not running
            if (!_soOrchestrator.TimerIsRunning)
            {
                mainForm.ToggleWorking(true);
                _initConfig();
                _loadConfig();
                _soOrchestrator.Initialize();
                _setClocks(_getWorkTimeFromSettings());
                mainForm.ToggleWorking(false);
            }
            else
            {
                _loadConfig();
            }
        }

        private void _showMainForm()
        {
            mainForm.Initialize();
            mainForm.Show();
        }

        private static void _exit()
        {
            logger.Debug("exit");
#if !DEBUG
            if (MessageBox.Show("Exit TimeNazi?", "Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                Application.Exit();
            }
#else
            Application.Exit();
#endif
        }

        #region EventHandlers

        private void _cfClockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null == sender)
            {
                return;
            }
            _cmConfigManager.Config.ClockFormPosition = ((Form)sender).Location;
            _cmConfigManager.SaveLocal();
        }
        private void _cfClockForm_Shown(object sender, EventArgs e)
        {
            if (null != sender)
            {
                ((Form)sender).Location = _cmConfigManager.Config.ClockFormPosition;
            }
        }


        void _soOrchestrator_WorkElapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info(string.Format("work period ended with {0}s, starting rest period", e.ElapsedTime));
            _logActivity(ActivityType.WorkEnd, e.ElapsedTime);
            _logActivity(ActivityType.RestBegin, 0);
            _scenarioResting();
        }

        void _soOrchestrator_SnoozeElapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info(string.Format("snooze period ended with {0}s, starting rest period", e.ElapsedTime));
            _logActivity(ActivityType.SnoozeEnd, e.ElapsedTime);
            _logActivity(ActivityType.RestBegin, 0);
            _scenarioResting();
        }

        void _soOrchestrator_RestElapsed(object sender, ElapsedEventArgs e)
        {
            logger.Info(string.Format("rest period ended with {0}s, starting work period", e.ElapsedTime));
            _logActivity(ActivityType.RestEnd, e.ElapsedTime);
            _logActivity(ActivityType.WorkBegin, 0);
            _scenarioWorking();
        }

        void _mfMainForm_SnoozeClicked(object sender, EventArgs e)
        {
            logger.Info(string.Format("snooze clicked, rest period suspended with {0}s elapsed", _lSOElapsedTime));
            _logActivity(ActivityType.RestEnd, _lSOElapsedTime);
            _logActivity(ActivityType.SnoozeBegin, 0);
            _soOrchestrator.StartSnooze();
            _scenarioWorking();
        }

        void _mfMainForm_StartClicked(object sender, EventArgs e)
        { // when the start button is clicked, start the scenario
            logger.Debug("start work clicked");
            _logActivity(ActivityType.WorkBegin, 0);
            _soOrchestrator.StartWork();
            _scenarioWorking();
        }

        void _sfSettingsForm_ClockOpacityChanged(object sender, OpacityEventArgs e)
        {
            _setClockOpacity(e.Opacity);
        }

        void _mfMainForm_SettingsClicked(object sender, EventArgs e)
        {
            _showSettingsForm(true);
        }

        //void _tTimer_Tick(object sender, EventArgs e)
        //{
        //    _timerElapsed();
        //}

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            _niTrayicon.Visible = false;
        }

        void _tsmiMenuItemShowClock_CheckStateChanged(object sender, EventArgs e)
        {
            _changeClockVisibility(sender);
        }

        void _tsmiMenuItemSettings_Click(object sender, EventArgs e)
        {
            _showSettingsForm(false);
        }

        void _tsmiMenuItemClose_Click(object sender, EventArgs e)
        {
            _exit();
        }

        void _niTrayicon_MouseUp(object sender, MouseEventArgs e)
        { // the context menu should pop up on left click too
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(_niTrayicon, null);
            }
        }
        #endregion

        private void InitializeComponent()
        {
            _niTrayicon = new NotifyIcon();
            _niTrayicon.BalloonTipIcon = ToolTipIcon.Info;
            _niTrayicon.BalloonTipTitle = "TimeNazi";
            _niTrayicon.BalloonTipText = "TimeNazi has been launched!";
            _niTrayicon.Text = "TimeNazi";
            _niTrayicon.Icon = Properties.Resources.clock_16;
            _niTrayicon.MouseUp += _niTrayicon_MouseUp;
            _niTrayicon.Visible = true;

            _cmsTrayIconContextMenu = new ContextMenuStrip();
            _cmsTrayIconContextMenu.SuspendLayout();

            //
            // MenuItemClose
            //
            _tsmiMenuItemExit = new ToolStripMenuItem();
            _tsmiMenuItemExit.Name = "MenuItemClose";
            _tsmiMenuItemExit.Text = "Exit";
            _tsmiMenuItemExit.Click += _tsmiMenuItemClose_Click;
            //
            // MenuItemSettings
            //
            _tsmiMenuItemSettings = new ToolStripMenuItem();
            _tsmiMenuItemSettings.Name = "MenuItemSettings";
            _tsmiMenuItemSettings.Text = "Settings";
            _tsmiMenuItemSettings.Click += _tsmiMenuItemSettings_Click;
            //
            // _tssSeparator
            //
            _tssSeparator = new ToolStripSeparator();
            //
            // MenuItemShowClock
            //
            _tsmiMenuItemShowClock = new ToolStripMenuItem();
            _tsmiMenuItemShowClock.Text = "Show clock";
            _tsmiMenuItemShowClock.CheckOnClick = true;
            //
            // _tsmiMenuItemEnable
            //
            _tsmiMenuItemEnable = new ToolStripMenuItem();
            _tsmiMenuItemEnable.Text = "Enabled";
            _tsmiMenuItemEnable.CheckOnClick = true;
            _tsmiMenuItemEnable.Checked = true;

            //
            // TrayIconContextMenu
            //
            _cmsTrayIconContextMenu.Items.AddRange(new ToolStripItem[] { _tsmiMenuItemSettings, _tsmiMenuItemShowClock, _tsmiMenuItemEnable, _tssSeparator, _tsmiMenuItemExit });
            _cmsTrayIconContextMenu.Name = "TrayIconContextMenu";

            _cmsTrayIconContextMenu.ResumeLayout(false);
            _niTrayicon.ContextMenuStrip = _cmsTrayIconContextMenu;

            //_niTrayicon.BalloonTipIcon = ToolTipIcon.Warning;
            //_niTrayicon.BalloonTipText = "Last minute!";
            //_niTrayicon.BalloonTipTitle = "Warning!";
            //_niTrayicon.ShowBalloonTip(10000);
        }
        private void InitializeComponent_needsConfig()
        {
            _tsmiMenuItemShowClock.Checked = _cmConfigManager.Config.ShowClock;
            _tsmiMenuItemShowClock.CheckStateChanged += _tsmiMenuItemShowClock_CheckStateChanged;
            _tsmiMenuItemEnable.CheckedChanged += _tsmiMenuItemEnable_CheckedChanged;
        }

        void _tsmiMenuItemEnable_CheckedChanged(object sender, EventArgs e)
        {
            _soOrchestrator.Enabled = ((ToolStripMenuItem)sender).Checked;
        }
    }
}