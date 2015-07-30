using Google.GData.Client;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TimeNazi
{
    public class ActivityLogManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public IDatabaseWrapper Database { get; set; }
        private string _sLogFileName = string.Empty;
        public string LogFileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_sLogFileName))
                {
                    _sLogFileName = Path.Combine(Constants.ApplicationDataDir
                                               , Constants.FILESYSTEM_ACTIVITYLOGDIR
                                               , string.Format(Constants.FILESYSTEM_ACTIVITYLOGS, DateTime.Now.ToString("yyyyMMdd"))
                                                );
                }
                if (!File.Exists(_sLogFileName))
                {
                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(_sLogFileName));
                        File.Create(_sLogFileName).Close();
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Error while creating activity log file (\"{1}\"): {0}", ex.Message, _sLogFileName);
                    }
                    //Thread.Sleep(10);
                }

                return _sLogFileName;
            }
        }


        public ActivityLogManager()
        {

        }

        public void Initialize(OAuth2Parameters authParams)
        {
            Database.Initialize(authParams);
        }

        public void StoreActivity(ActivityLogEntry entry)
        {
            try
            {
                File.AppendAllLines(LogFileName, new string[] { entry.ToString() });
                if (null != Database)
                {
                    Database.StoreActivity(entry);
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while storing activity ({1}): {0}", ex.Message, entry.ToString());
            }
        }
    }
}
