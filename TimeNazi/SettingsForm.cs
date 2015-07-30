using Microsoft.Win32;
using NLog;
using nUtils.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeNazi
{
    public partial class SettingsForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public delegate void ClockOpacityEventHandler(object sender, OpacityEventArgs e);
        public event ClockOpacityEventHandler ClockOpacityChanged;

        public ConfigurationManager ConfigManager { get; set; }
        public GoogleApiWrapper GoogleWrapper { get; set; }

        private FileListItem[] _fliaFileList = new FileListItem[0];
        private CancellationTokenSource _ctsCancellationTokenSource = new CancellationTokenSource();

        public SettingsForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.clock;
            //_loadSettings();
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            _saveSettings();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _loadSettings();
            this.Hide();
        }


        private void tbClockOpacity_Scroll(object sender, EventArgs e)
        {
            _updateTrackBarValueDisplay();
        }

        private void tbClockOpacity_ValueChanged(object sender, EventArgs e)
        {
            _updateTrackBarValueDisplay();
        }
        private void btnDefaults_Click(object sender, EventArgs e)
        {
            _setDefaults();
        }

        private void _loadSettings()
        {
            logger.Debug("Loading settings");
            // load the settings
            nudWorkTime.Value = ConfigManager.Config.WorkTime;
            nudSnoozeTime.Value = ConfigManager.Config.SnoozeTime;
            nudRestTime.Value = ConfigManager.Config.RestTime;
            tbClockOpacity.Value = ConfigManager.Config.ClockOpacity;
            cbStartWithWindows.Checked = _getStartWithWindows();
            cbUseGoogleDrive.Checked = ConfigManager.Config.UseDataBase;
            txtGoogleKeyFile.Text = ConfigManager.Config.DatabaseKeyFile;
            _toggleUseDatabase(ConfigManager.Config.UseDataBase);
        }

        private void _saveSettings()
        {
            logger.Debug("Saving settings");
            // save the settings
            ConfigManager.Config.ClockOpacity = tbClockOpacity.Value;
            ConfigManager.Config.WorkTime = (int)nudWorkTime.Value;
            ConfigManager.Config.SnoozeTime = (int)((nudSnoozeTime.Value > 20) ? 20 : nudSnoozeTime.Value);
            ConfigManager.Config.RestTime = (int)nudRestTime.Value;
            //ConfigManager.Config.ShowClock
            ConfigManager.Config.UseDataBase = cbUseGoogleDrive.Checked;
            if (!cbUseGoogleDrive.Checked)
            {
                txtGoogleKeyFile.Text = string.Empty;
            }
            ConfigManager.Config.DatabaseKeyFile = txtGoogleKeyFile.Text;
            this.Enabled = false;
            ConfigManager.Save();
            _setStartWithWindows(cbStartWithWindows.Checked);
            this.Enabled = true;

            // close the window
            this.Hide();
        }

        private void _setDefaults()
        {
            logger.Debug("Resetting to defaults");
            nudWorkTime.Value = BaseConfiguration.Default.WorkTime;
            nudSnoozeTime.Value = BaseConfiguration.Default.SnoozeTime;
            nudRestTime.Value = BaseConfiguration.Default.RestTime;
            tbClockOpacity.Value = BaseConfiguration.Default.ClockOpacity;
            cbUseGoogleDrive.Checked = false;
        }


        private void _updateTrackBarValueDisplay()
        {
            lblClockOpacity.Text = tbClockOpacity.Value + "%";
            if (null != ClockOpacityChanged)
            {
                ClockOpacityChanged(this, new OpacityEventArgs() { Opacity = tbClockOpacity.Value });
            }
        }

        private void _setStartWithWindows(bool enabled)
        {
            logger.Debug("_setStartWithWindows={0}", enabled);
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.REGISTRYKEY_AUTOSTART, true);
                if (enabled)
                {
                    key.SetValue(Constants.REGISTRYKEY_KEYNAME, Application.ExecutablePath.ToString());
                }
                else
                {
                    key.DeleteValue(Constants.REGISTRYKEY_KEYNAME, false);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while {2} registry key \"{1}\": {0}", ex.Message, Constants.REGISTRYKEY_AUTOSTART, enabled ? "setting" : "deleting");
            }
        }

        private bool _getStartWithWindows()
        {
            logger.Debug("_getStartWithWindows");
            string sValue = string.Empty;
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(Constants.REGISTRYKEY_AUTOSTART, false);
                sValue = key.GetValue(Constants.REGISTRYKEY_KEYNAME, string.Empty) as string;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while reading registry \"{1}\": {0}", ex.Message, Constants.REGISTRYKEY_AUTOSTART);
            }

            return !string.IsNullOrEmpty(sValue);
        }

        private void _loadGoogleKeyFile()
        {
            logger.Debug("_loadGoogleKeyFile");
            if (ofdDatabaseFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                logger.Debug("Selected file {0}", ofdDatabaseFileDialog.FileName);
                if (!File.Exists(ofdDatabaseFileDialog.FileName))
                {
                    MessageBox.Show("Selected file does not exist!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    txtGoogleKeyFile.Text = ofdDatabaseFileDialog.FileName;
                    try
                    {
                        GoogleWrapper.LoadAuthParams(ofdDatabaseFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while loading key file: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtGoogleKeyFile.Text = string.Empty;
                    }
                    _updateDBStatusText();
                }
            }
        }

        private void _toggleUseDatabase(bool useEnabled)
        {
            logger.Debug("_toggleUseDatabase={0}", useEnabled);
            if (useEnabled)
            {
                lblKeyFileLabel.Enabled = true;
                lblGoogleStatus.Enabled = true;
                lblStatusLabel.Enabled = true;
                txtGoogleKeyFile.Enabled = true;
                btnBrowseGoogleKeyFile.Enabled = true;
            }
            else
            {
                lblKeyFileLabel.Enabled = false;
                lblGoogleStatus.Enabled = false;
                lblStatusLabel.Enabled = false;
                txtGoogleKeyFile.Enabled = false;
                btnBrowseGoogleKeyFile.Enabled = false;
                btnAuthorizeWithGoogle.Enabled = false;
                btnTestGoogleConnection.Enabled = false;
            }
            _updateDBStatusText();
        }

        private void _updateDBStatusText()
        {
            logger.Debug("_updateDBStatusText");
            btnAuthorizeWithGoogle.Enabled = false;
            btnTestGoogleConnection.Enabled = false;
            Color cColor = Color.Black;
            string sText = string.Empty;
            if (cbUseGoogleDrive.Checked)
            {
                if (string.IsNullOrWhiteSpace(txtGoogleKeyFile.Text))
                {
                    cColor = Color.Orange;
                    sText = "key file not loaded";
                }
                else
                {
                    if (GoogleWrapper.NeedsAuthorization)
                    {
                        cColor = Color.Orange;
                        sText = "needs authorization";
                        btnAuthorizeWithGoogle.Enabled = true;
                        btnTestGoogleConnection.Enabled = false;
                    }
                    else
                    {
                        cColor = Color.Green;
                        sText = "ok";
                        btnAuthorizeWithGoogle.Enabled = false;
                        btnTestGoogleConnection.Enabled = true;
                    }
                }
            }
            else
            {
                sText = "disabled";
                cColor = Color.Gray;
            }
            lblGoogleStatus.Text = sText;
            lblGoogleStatus.ForeColor = cColor;
        }

        private void btnBrowseGoogleKeyFile_Click(object sender, EventArgs e)
        {
            _loadGoogleKeyFile();
        }

        private void cbUseGoogleDrive_CheckedChanged(object sender, EventArgs e)
        {
            _toggleUseDatabase(((CheckBox)sender).Checked);
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            _loadSettings();
        }

        private void btnAuthorizeWithGoogle_Click(object sender, EventArgs e)
        {
            logger.Debug("btnAuthorizeWithGoogle_Click {0}", GoogleWrapper.NeedsAuthorization);
            if (GoogleWrapper.NeedsAuthorization)
            {
                if (!GoogleWrapper.ShowAuthorizationDialog(true))
                {
                    //MessageBox.Show("Error while authorizing application!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Error("Error while authorizing application!");
                }
                _updateDBStatusText();
            }
        }

        private void btnTestGoogleConnection_Click(object sender, EventArgs e)
        {
            logger.Debug("btnTestGoogleConnection_Click");
            if (!GoogleWrapper.NeedsAuthorization)
            {
                Exception ex = GoogleWrapper.TestConnection();
                if (null != ex)
                {
                    MessageBox.Show(string.Format("{0}{1}", ex.Message, ((null != ex.InnerException) ? ", " + ex.InnerException.Message : "")), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Test successfull!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                _updateDBStatusText();
            }
        }

        private void btnCheckBackgroundImages_Click(object sender, EventArgs e)
        {
            _checkBackgroundImageSource();
        }

        private void _checkBackgroundImageSource()
        {
            _fliaFileList = new FileListItem[0];
            _ctsCancellationTokenSource = new CancellationTokenSource();
            _ctsCancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(30000));
            _toggleCancelButton(true);
            Task tsk = Task.Factory.StartNew(async () =>
            {
                try
                {
                    _changeBackgroundImagesStatusLabel("checking...", Color.Black);
                    _fliaFileList = await _checkBackgroundImages();
                    _updateBackgroundImageStatus();
                    //if (iNew > 0 || iChanged > 0)
                    //{
                    //    _toggleDownloadButton(true);
                    //}
                }
                catch (OperationCanceledException cex)
                {
                    _changeBackgroundImagesStatusLabel("unknown", Color.Black);
                    lnkCancelTask.Visible = false;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Background file list download error url=\"{0}\": {1}", ConfigManager.Config.BackgroundImagesSource, ex.Message);
                    MessageBox.Show(string.Format("Error while getting file list from \"{0}\": {1}", ConfigManager.Config.BackgroundImagesSource, ex.Message), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _changeBackgroundImagesStatusLabel("error", Color.Red);
                }
            }, _ctsCancellationTokenSource.Token).ContinueWith((t) =>
            {
                _toggleCancelButton(false);
            });
        }

        private void _updateBackgroundImageStatus()
        {
            int iNew = _fliaFileList.Count(f => f.Status == FileStatus.New);
            int iChanged = _fliaFileList.Count(f => f.Status == FileStatus.Updated);
            _changeBackgroundImagesStatusLabel(string.Format("{0} total, {1} new, {2} updated", _fliaFileList.Length, iNew, iChanged), Color.Black);
        }

        private async Task<FileListItem[]> _checkBackgroundImages()
        {
            List<FileListItem> lfliRet = new List<FileListItem>();
            string sFileListURL = ConfigManager.Config.BackgroundImagesSource;
            string sBaseURL = NetworkUtils.GetBaseURI(sFileListURL);

            string[] saFileList = await _getBackgroundImagesFileList(sFileListURL);
            if (saFileList.Length > 0)
            {
                foreach (string sFile in saFileList)
                {
                    FileListItem fliItem = new FileListItem();
                    if (string.IsNullOrWhiteSpace(sFile))
                    {
                        continue;
                    }
                    string[] sa = sFile.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (null == sa || sa.Length < 2)
                    {
                        continue;
                    }
                    fliItem.RemoteFileName = sBaseURL + sa[0];
                    fliItem.RemoteHash = sa[1];
                    fliItem.LocalFileName = Path.Combine(Constants.ApplicationDataDir, Constants.FILESYSTEM_BACKGROUNDIMAGES, Path.GetFileName(fliItem.RemoteFileName));
                    fliItem.LocalHash = _getFileHash(fliItem.LocalFileName);

                    lfliRet.Add(fliItem);
                }
            }

            return lfliRet.ToArray();
        }
        private async Task<string[]> _getBackgroundImagesFileList(string ulr)
        {
            //Task.Delay(15000).Wait(_ctsCancellationTokenSource.Token);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(ulr);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(result))
            {
                return new string[0];
            }
            string[] saLines = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return saLines;
        }

        private string _getFileHash(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                return string.Empty;
            }
            using (var md5 = MD5.Create())
            {
                using (var stream = System.IO.File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
                }
            }
        }

        private void lnkCancelTask_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _ctsCancellationTokenSource.Cancel();
        }

        private void _toggleCancelButton(bool visible)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { lnkCancelTask.Visible = visible; }));
            }
            else
            {
                lnkCancelTask.Visible = visible;
            }
        }

        //private void _toggleDownloadButton(bool enabled)
        //{
        //    if (InvokeRequired)
        //    {
        //        Invoke((MethodInvoker)(() => { btnDownloadBackgroundImages.Enabled = enabled; }));
        //    }
        //    else
        //    {
        //        btnDownloadBackgroundImages.Enabled = enabled;
        //    }
        //}

        private void _changeBackgroundImagesStatusLabel(string text, Color color)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)(() => { lblBackgroundImagesStatus.Text = text; lblBackgroundImagesStatus.ForeColor = color; }));
            }
            else
            {
                lblBackgroundImagesStatus.Text = text;
                lblBackgroundImagesStatus.ForeColor = color;
            }
        }

        private void btnDownloadBackgroundImages_Click(object sender, EventArgs e)
        {
            if (null == _fliaFileList || _fliaFileList.Length == 0)
            {
                MessageBox.Show("Run check first!", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _ctsCancellationTokenSource = new CancellationTokenSource();
            _toggleCancelButton(true);
            Task tsk = Task.Factory.StartNew(async () =>
            {
                try
                {
                    _changeBackgroundImagesStatusLabel("downloading...", Color.Black);
                    await _downloadBackgroundImages(_fliaFileList);
                }
                catch (OperationCanceledException cex)
                {
                    lnkCancelTask.Visible = false;
                    _updateBackgroundImageStatus();
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Background file download error url=\"{0}\": {1}", ConfigManager.Config.BackgroundImagesSource, ex.Message);
                    MessageBox.Show(string.Format("Error while downloading background images: {1}", ConfigManager.Config.BackgroundImagesSource, ex.Message), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _changeBackgroundImagesStatusLabel("error", Color.Red);
                }
            }, _ctsCancellationTokenSource.Token).ContinueWith((t) =>
            {
                _toggleCancelButton(false);
                //_checkBackgroundImageSource();
                _changeBackgroundImagesStatusLabel("done.", Color.Black);
            });
        }

        private async Task _downloadBackgroundImages(FileListItem[] files)
        {
            List<Task> ltDownloadList = new List<Task>();
            foreach (var oneFile in files)
            {
                Console.WriteLine(oneFile);
                ltDownloadList.Add(_saveFileToDisk(oneFile.RemoteFileName, oneFile.LocalFileName));
            }
            await Task.WhenAll(ltDownloadList);
        }

        private async Task _saveFileToDisk(string fileUrl, string filePath)
        {
            //Task.Delay(5000).Wait(_ctsCancellationTokenSource.Token);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(fileUrl);
            response.EnsureSuccessStatusCode();
            byte[] result = await response.Content.ReadAsByteArrayAsync();
            System.IO.File.WriteAllBytes(filePath, result);
        }
    }

    public class OpacityEventArgs : EventArgs
    {
        public int Opacity { get; set; }
    }

    public enum SettingsSenderType
    {
        Database,
        BackgroundImages
    }

    public class FileListItem
    {
        public string RemoteFileName { get; set; }
        public string LocalFileName { get; set; }
        public string RemoteHash { get; set; }
        public string LocalHash { get; set; }
        public FileStatus Status
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LocalHash))
                {
                    return FileStatus.New;
                }
                else if (0 != string.Compare(LocalHash, RemoteHash, true))
                {
                    return FileStatus.Updated;
                }
                else
                {
                    return FileStatus.NotChanged;
                }
            }
        }
    }

    public enum FileStatus
    {
        New,
        Updated,
        NotChanged
    }
}
