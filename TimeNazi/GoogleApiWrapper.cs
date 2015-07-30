using Google.GData.Client;
using Google.GData.Spreadsheets;
using NLog;
using nUtils.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TimeNazi
{
    public class GoogleApiWrapper : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public OAuth2Parameters AuthParams { get; set; }
        public event EventHandler TokenRefreshed;
        public bool NeedsAuthorization
        {
            get { return string.IsNullOrWhiteSpace(AuthParams.AccessToken); }
        }
        public int RefreshSecsBeforeExpiryDate { get; set; }

        private Thread _thTokenRefreshThread;
        private string _sAuthParamFileName = string.Empty;
        private static object _oLock = new object();
        //private static AuthResult _arAuthResult;

        public GoogleApiWrapper()
        {
            logger.Debug("GoogleApiWrapper ctor");
            AuthParams = new OAuth2Parameters();
            _thTokenRefreshThread = new Thread(new ThreadStart(_threadAction));
            _thTokenRefreshThread.Start();
        }

        public void LoadAuthParams(string fileName)
        {
            try
            {
                logger.Info("Loading auth params: {0}", fileName);
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentNullException("fileName", "Filename cannot be empty!");
                }
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException(string.Format("File \"{0}\" does not exist!", fileName), fileName);
                }
                _sAuthParamFileName = fileName;
                string sDecrypted = string.Empty;
                using (StreamReader srReader = new StreamReader(_sAuthParamFileName))
                {
                    string sEncrypted = srReader.ReadToEnd();
                    logger.Debug("Loaded file. length:{0}", sEncrypted.Length);
                    if (string.IsNullOrWhiteSpace(sEncrypted))
                    {
                        throw new ArgumentNullException("File is empty!");
                    }
                    //sDecrypted = CryptoUtils.DecryptString(sEncrypted);
                    // HACK - ProtectedData.Protect only works on the same machine. don't need anything extra, so a base64+rot13 will do... :(
                    sDecrypted = CryptoUtils.SimpleDecrypt(sEncrypted);
                }
                using (MemoryStream msStream = new MemoryStream(Encoding.UTF8.GetBytes(sDecrypted)))
                {
                    logger.Debug("Decrypted file. length:{0}", msStream.Length);
                    if (msStream.Length == 0)
                    {
                        throw new ArgumentException("Not a valid key file! (Decrypt error)");
                    }
                    XmlSerializer ser = new XmlSerializer(AuthParams.GetType());
                    AuthParams = (OAuth2Parameters)ser.Deserialize(msStream);
                }
                logger.Debug("Loading done");
                if (!NeedsAuthorization)
                {
                    _refreshToken();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while loading OAuth2Parameters from file \"{0}\": {1}", fileName, ex.Message);
                throw new Exception(string.Format("Error while loading OAuth2Parameters from file \"{0}\": {1}", fileName, ex.Message), ex);
            }
        }

        public void SaveAuthParams()
        {
            try
            {
                logger.Info("Saving auth params: {0}", _sAuthParamFileName);
                if (string.IsNullOrWhiteSpace(_sAuthParamFileName))
                {
                    throw new ArgumentNullException("fileName", "Filename cannot be empty!");
                }
                using (MemoryStream msStream = new MemoryStream())
                { // serialize into the memory stream
                    XmlSerializer ser = new XmlSerializer(AuthParams.GetType());
                    ser.Serialize(msStream, AuthParams);
                    msStream.Position = 0;

                    // encrypt
                    //string sEncrypted = CryptoUtils.EncryptString(msStream.ToArray());
                    string sEncrypted = CryptoUtils.SimpleEncrypt(msStream.ToArray());
                    // write to file
                    using (StreamWriter swWriter = new StreamWriter(_sAuthParamFileName, false))
                    {
                        swWriter.Write(sEncrypted);
                    }
                }
                logger.Debug("Saving done");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while saving OAuth2Parameters into file \"{0}\": {1}", _sAuthParamFileName, ex.Message);
                throw new Exception(string.Format("Error while saving OAuth2Parameters into file \"{0}\": {1}", _sAuthParamFileName, ex.Message), ex);
            }
        }

        public bool ShowAuthorizationDialog(bool isModal)
        {
            logger.Debug("ShowAuthorizationDialog {0}", isModal);
            string sAuthorizationURL = OAuthUtil.CreateOAuth2AuthorizationUrl(AuthParams);
            GoogleAuthForm fAuthForm = new GoogleAuthForm();
            fAuthForm.Url = sAuthorizationURL;
            if (isModal)
            {
                fAuthForm.ShowDialog();
            }
            else
            {
                fAuthForm.Show();
            }
            AuthResult arAuthResult = fAuthForm.Result ?? new AuthResult() { Result = ResultType.Denied, Value = string.Empty };
            logger.Info("Authorization Result={0}, Value={1}", arAuthResult.Result, arAuthResult.Value);

            if (arAuthResult.Result == ResultType.Success)
            {
                AuthParams.AccessCode = arAuthResult.Value;
                OAuthUtil.GetAccessToken(AuthParams);

                SaveAuthParams();

                return true;
            }

            return false;
        }

        public Exception TestConnection()
        {
            logger.Debug("TestConnection");
            if (null != AuthParams && !NeedsAuthorization)
            {
                if (AuthParams.Scope.Contains("https://spreadsheets.google.com/feeds"))
                {
                    try
                    {
                        GOAuth2RequestFactory rfRequestFactory = new GOAuth2RequestFactory(null, "GoogleApiWrapper", AuthParams);
                        SpreadsheetsService ssSpreadesheetService = new SpreadsheetsService("GoogleApiWrapper");
                        ssSpreadesheetService.RequestFactory = rfRequestFactory;

                        // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
                        SpreadsheetQuery query = new SpreadsheetQuery();

                        // Make a request to the API and get all spreadsheets.
                        SpreadsheetFeed feed = ssSpreadesheetService.Query(query);

                        if (null != feed)
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Test connection resulted in an error: {0}", ex.Message);
                        return ex;
                    }
                }
                else
                {
                    throw new NotImplementedException("Only the spreadsheet test is implemented! (Add that to the scope!)");
                }
            }

            return null;
        }

        private void _refreshToken()
        {
            logger.Trace("_refreshToken AuthParamsIsNull={0}, NeedsAuthorization={1}", null == AuthParams, NeedsAuthorization);
            if (null != AuthParams && !NeedsAuthorization)
            {
                logger.Trace("AuthParams.TokenExpiry={0}", AuthParams.TokenExpiry);
                if (AuthParams.TokenExpiry <= DateTime.Now.AddSeconds(-1 * RefreshSecsBeforeExpiryDate))
                {
                    logger.Debug("Access token expired on {0}. Refreshing...", AuthParams.TokenExpiry);
                    OAuthUtil.RefreshAccessToken(AuthParams);
                    logger.Info("New token expiry is: {0}", AuthParams.TokenExpiry);
                    SaveAuthParams();
                    if (null != TokenRefreshed)
                    {
                        TokenRefreshed(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void _threadAction()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(10000);
                    _refreshToken();
                }
            }
            catch (ThreadAbortException)
            { }
            catch (Exception ex)
            {
                logger.Error(ex, "_threadAction encountered an exception: {0}", ex.Message);
            }
        }

        //static void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    WebBrowser wb = (WebBrowser)sender;
        //    Console.WriteLine(wb.DocumentText);

        //    if (wb.DocumentTitle.ToLower().StartsWith("Denied".ToLower()))
        //    { // there was an error
        //        string sErrorMessage = wb.DocumentTitle.Substring(wb.DocumentTitle.IndexOf("error=") + 6);
        //        lock (_oLock)
        //        {
        //            _arAuthResult = new AuthResult() { Result = ResultType.Denied, Value = sErrorMessage };
        //        }
        //        Thread.CurrentThread.Abort();
        //    }
        //    else if (wb.DocumentTitle.ToLower().StartsWith("Success".ToLower()))
        //    { // there was an error
        //        string sCodePart = wb.DocumentTitle.Substring(wb.DocumentTitle.IndexOf("code=") + 5);
        //        var x = wb.Document.GetElementById("code");
        //        string sCode = x.GetAttribute("value");
        //        lock (_oLock)
        //        {
        //            _arAuthResult = new AuthResult() { Result = ResultType.Success, Value = sCode };
        //        }
        //        Thread.CurrentThread.Abort();
        //    }
        //}

        public void Dispose()
        {
            if (null != _thTokenRefreshThread)
            {
                try
                {
                    _thTokenRefreshThread.Abort();
                }
                catch
                { }
            }
        }
    }

    public enum ResultType
    {
        Success,
        Denied
    }
    public class AuthResult
    {
        public ResultType Result { get; set; }
        public string Value { get; set; }
    }

    public class GoogleAuthForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private WebBrowser wbBrowser;
        public string Url { get; set; }
        public AuthResult Result { get; set; }

        public GoogleAuthForm()
        {
            this.Name = "GoogleAuthForm";
            this.Text = "Authorize application";
            this.Width = 515;
            this.Height = 739;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = Properties.Resources.clock;
            this.SuspendLayout();
            wbBrowser = new WebBrowser();
            wbBrowser.Name = "BROwser";
            wbBrowser.Width = 500;
            wbBrowser.Height = 700;
            wbBrowser.ScrollBarsEnabled = true;
            wbBrowser.AllowNavigation = true;
            wbBrowser.DocumentCompleted += wbBrowser_DocumentCompleted;
            this.Controls.Add(wbBrowser);
            this.ResumeLayout();
            this.Shown += GoogleAuthForm_Shown;
        }

        void GoogleAuthForm_Shown(object sender, EventArgs e)
        {
            wbBrowser.Navigate(Url);
        }

        private void wbBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = (WebBrowser)sender;

            logger.Debug("GoogleAuthForm={0}", wb.DocumentTitle);

            if (wb.DocumentTitle.ToLower().StartsWith("Denied".ToLower()))
            { // there was an error
                string sErrorMessage = wb.DocumentTitle.Substring(wb.DocumentTitle.IndexOf("error=") + 6);
                Result = new AuthResult() { Result = ResultType.Denied, Value = sErrorMessage };
                this.Close();
            }
            else if (wb.DocumentTitle.ToLower().StartsWith("Success".ToLower()))
            { // there was an error
                string sCodePart = wb.DocumentTitle.Substring(wb.DocumentTitle.IndexOf("code=") + 5);
                var x = wb.Document.GetElementById("code");
                string sCode = x.GetAttribute("value");
                Result = new AuthResult() { Result = ResultType.Success, Value = sCode };
                this.Close();
            }
        }
    }
}
/*
 
        fForm.Width = 515;
    fForm.Height = 739;
    using (WebBrowser wbBrowser = new WebBrowser())
    {
        fForm.Controls.Add(wbBrowser);
        wbBrowser.Width = 500;
        wbBrowser.Height = 700;
        wbBrowser.ScrollBarsEnabled = true;
        wbBrowser.AllowNavigation = true;
        wbBrowser.DocumentCompleted += wbBrowser_DocumentCompleted;
        if (isModal)
        {
            fForm.ShowDialog();
        }
        else
        {
            fForm.Show();
        }
        wbBrowser.Navigate(obj as string);
        while (true)
        {
            System.Windows.Forms.Application.DoEvents();
        }
    }

 */