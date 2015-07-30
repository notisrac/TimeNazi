using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TimeNazi
{
    public class Constants
    {
        public const string FILESYSTEM_BACKGROUNDIMAGES = "Backgrounds";
        public const string FILESYSTEM_ACTIVITYLOGS = "activity_{0}.csv";
        public const string FILESYSTEM_SYSLOGS = "applog.log";
        public const string FILESYSTEM_LOGDIR = "Logs";
        public const string FILESYSTEM_ACTIVITYLOGDIR = "ActivityLogs";

        public const string REGISTRYKEY_AUTOSTART = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        public const string REGISTRYKEY_KEYNAME = "TimeNazi";

        public const string DATABASE_NAME = "TimeNaziDB";
        public const string DATABASE_CONFIGSHEET = "Config";
        public const string DATABASE_LOGSHEET = "ActivityLog";

        // TODO is this ok? (static?)
        private static string _sApplicationDataDir = string.Empty;
        public static string ApplicationDataDir
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sApplicationDataDir))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
                    var companyName = versionInfo.CompanyName;
                    var appName = versionInfo.ProductName;
                    _sApplicationDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), companyName, appName);
                }
                return _sApplicationDataDir;
            }
        }

    }
}
