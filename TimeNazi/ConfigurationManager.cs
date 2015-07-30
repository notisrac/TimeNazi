using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNazi
{
    public class ConfigurationManager
    {
        public Configuration Config { get; set; }
        public IDatabaseWrapper Database { get; set; }

        public ConfigurationManager()
        {
            //Config = new Configuration();
            Config = new Configuration(BaseConfiguration.Default);
        }

        public void Load()
        {
            // offline config first
            LoadLocal();

            if (null != Database && Config.UseDataBase)
            { // load the online config, and overwrite the local with it
                IConfiguration confDBConfig = Database.LoadConfig();
                Config.ClockOpacity = confDBConfig.ClockOpacity;
                Config.NumberOfSnoozes = confDBConfig.NumberOfSnoozes;
                Config.PauseTimeout = confDBConfig.PauseTimeout;
                Config.RestTime = confDBConfig.RestTime;
                Config.ShowClock = confDBConfig.ShowClock;
                Config.SnoozeTime = confDBConfig.SnoozeTime;
                Config.WorkTime = confDBConfig.WorkTime;

                // update the local config with the online
                SaveLocal();
            }
        }

        public void LoadLocal()
        {
            Config.WorkTime = Properties.Settings.Default.WorkTime;
            Config.SnoozeTime = Properties.Settings.Default.SnoozeTime;
            Config.RestTime = Properties.Settings.Default.RestTime;
            //Config.DatabaseUsername = Properties.Settings.Default.DatabaseUsername;
            //Config.DatabasePassword = Properties.Settings.Default.DatabasePassword;
            Config.ShowClock = Properties.Settings.Default.ShowClock;
            Config.ClockOpacity = Properties.Settings.Default.ClockOpacity;
            Config.NumberOfSnoozes = Properties.Settings.Default.NumberOfSnoozes;
            Config.PauseTimeout = Properties.Settings.Default.PauseTimeout;
            Config.UseDataBase = Properties.Settings.Default.UseDataBase;
            Config.DatabaseKeyFile = Properties.Settings.Default.DatabaseKeyFile;
            Config.BackgroundImagesSource = Properties.Settings.Default.BackgroundImagesSource;
            Config.ClockFormPosition = Properties.Settings.Default.ClockFormPosition;
        }

        public void Save()
        {
            if (null != Database && Config.UseDataBase)
            {
                Database.SaveConfig(Config);
            }
            // always save the local config
            SaveLocal();
        }

        public void SaveLocal()
        {
            Properties.Settings.Default.WorkTime = Config.WorkTime;
            Properties.Settings.Default.SnoozeTime = Config.SnoozeTime;
            Properties.Settings.Default.RestTime = Config.RestTime;
            //Properties.Settings.Default.DatabaseUsername = Config.DatabaseUsername;
            //Properties.Settings.Default.DatabasePassword = Config.DatabasePassword;
            Properties.Settings.Default.ShowClock = Config.ShowClock;
            Properties.Settings.Default.ClockOpacity = Config.ClockOpacity;
            Properties.Settings.Default.NumberOfSnoozes = Config.NumberOfSnoozes;
            Properties.Settings.Default.PauseTimeout = Config.PauseTimeout;
            Properties.Settings.Default.UseDataBase = Config.UseDataBase;
            Properties.Settings.Default.DatabaseKeyFile = Config.DatabaseKeyFile;
            Properties.Settings.Default.ClockFormPosition = Config.ClockFormPosition;
            Properties.Settings.Default.Save();
        }
    }
}
