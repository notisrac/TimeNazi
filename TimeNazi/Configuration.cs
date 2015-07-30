using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNazi
{
    public class Configuration : BaseConfiguration
    {
        //public string DatabaseUsername { get; set; }
        //public string DatabasePassword { get; set; }
        public bool UseDataBase { get; set; }
        public string DatabaseKeyFile { get; set; }
        public string BackgroundImagesSource { get; set; }
        public Point ClockFormPosition { get; set; }

        public Configuration()
        {

        }

        public Configuration(BaseConfiguration baseConfig)
        {
            if (null == baseConfig)
            {
                return;
            }

            this.ClockOpacity = baseConfig.ClockOpacity;
            this.DatabaseKeyFile = string.Empty;
            this.NumberOfSnoozes = baseConfig.NumberOfSnoozes;
            this.PauseTimeout = baseConfig.PauseTimeout;
            this.RestTime = baseConfig.RestTime;
            this.ShowClock = baseConfig.ShowClock;
            this.SnoozeTime = baseConfig.SnoozeTime;
            this.UseDataBase = false;
            this.WorkTime = baseConfig.WorkTime;
        }
    }
}
