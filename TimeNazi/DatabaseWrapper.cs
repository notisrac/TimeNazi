using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GDataDB;
using NLog;
using Google.GData.Client;


namespace TimeNazi
{
    public class DatabaseWrapper : TimeNazi.IDatabaseWrapper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IDatabaseClient _dbcClient;
        private IDatabase _dbDatabase;
        private ITable<DBConfiguration> _tblConfigTable;
        private ITable<ActivityLogEntry> _tblActivityLogTable;
        private bool _bIsInitialized = false;
        private OAuth2Parameters _oapAuthParams;

        public DatabaseWrapper(OAuth2Parameters authParams)
        {
            try
            {
                Initialize(authParams);
                logger.Debug("Opening database");
                _dbDatabase = _dbcClient.GetDatabase(Constants.DATABASE_NAME);
                if (null == _dbDatabase)
                {
                    logger.Info("Database does not exist. Creating it.");
                    _dbDatabase = _dbcClient.CreateDatabase(Constants.DATABASE_NAME);
                }
                _bIsInitialized = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to connect to the database: {0}", ex.Message);
            }
        }

        public void Initialize(OAuth2Parameters authParams)
        {
            _oapAuthParams = authParams;
            logger.Debug("Initializing: Connecting to Google spreadsheet services");
            _dbcClient = new DatabaseClient(_oapAuthParams);
        }

        public bool InitializeConfigTable()
        {
            if(!_bIsInitialized)
            {
                return false;
            }

            try
            {
                logger.Debug("Initializing the config table");
                _tblConfigTable = _dbDatabase.GetTable<DBConfiguration>(Constants.DATABASE_CONFIGSHEET);
                if (null == _tblConfigTable)
                {
                    logger.Info("Config table does not exist. Creating it.");
                    _tblConfigTable = _dbDatabase.CreateTable<DBConfiguration>(Constants.DATABASE_CONFIGSHEET);
                }
                logger.Debug("Checking the configuration row");
                IList<IRow<DBConfiguration>> rows = _tblConfigTable.FindAll();
                if (null == rows || rows.Count == 0)
                {
                    logger.Debug("Configuration row not found. Creating it.");
                    _tblConfigTable.Add(new DBConfiguration(BaseConfiguration.Default));
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to initialize the config table: {0}", ex.Message);
                _tblConfigTable = null;
                return false;
            }

            return true;
        }

        public bool InitializeActivityLogTable()
        {
            if (!_bIsInitialized)
            {
                return false;
            }

            try
            {
                logger.Debug("Initializing the activity log table");
                _tblActivityLogTable = _dbDatabase.GetTable<ActivityLogEntry>(Constants.DATABASE_LOGSHEET);
                if (null == _tblActivityLogTable)
                {
                    logger.Info("Activity log table does not exist. Creating it.");
                    _tblActivityLogTable = _dbDatabase.CreateTable<ActivityLogEntry>(Constants.DATABASE_LOGSHEET);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to initialize the activity log table: {0}", ex.Message);
                _tblActivityLogTable = null;
                return false;
            }

            return true;
        }

        public void SaveConfig(IConfiguration config)
        {
            if (null == _tblConfigTable)
            {
                return;
            }
            DBConfiguration dbConfig = new DBConfiguration(config);
            logger.Info("Updating config with the last modified date and the modifier");
            dbConfig.LastUpdated = DateTime.Now;
            dbConfig.LastUpdateSource = string.Format("{0}@{1}", Environment.UserName, Environment.MachineName);

            logger.Info("Saving config");
            IList<IRow<DBConfiguration>> rows = _tblConfigTable.FindAll();
            if (null != rows && rows.Count > 0)
            {
                rows[0].Element = dbConfig;
                rows[0].Update();
            }
            else
            {
                _tblConfigTable.Add(dbConfig);
            }
        }

        public IConfiguration LoadConfig()
        {
            if (null == _tblConfigTable)
            {
                return null;
            }

            logger.Info("Loading config");
            IList<IRow<DBConfiguration>> rows = _tblConfigTable.FindAll();
            if (null != rows && rows.Count > 0)
            {
                return rows[0].Element;
            }

            return null;
        }

        public void StoreActivity(ActivityLogEntry entry)
        {
            if (null == _tblActivityLogTable)
            {
                return;
            }

            _tblActivityLogTable.Add(entry);
        }
    }
}
