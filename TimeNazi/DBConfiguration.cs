using GDataDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNazi
{
    public class DBConfiguration : BaseConfiguration
    {
        public DateTime LastUpdated { get; set; }
        public string LastUpdateSource { get; set; }

        public DBConfiguration()
        {
        }

        public DBConfiguration(BaseConfiguration source)
        {
            this.ClockOpacity = source.ClockOpacity;
            this.LastUpdated = DateTime.MinValue;
            this.LastUpdateSource = string.Empty;
            this.NumberOfSnoozes = source.NumberOfSnoozes;
            this.PauseTimeout = source.PauseTimeout;
            this.RestTime = source.RestTime;
            this.ShowClock = source.ShowClock;
            this.SnoozeTime = source.SnoozeTime;
            this.WorkTime = source.WorkTime;
        }

        public DBConfiguration(Configuration source)
        {
            this.ClockOpacity = source.ClockOpacity;
            this.LastUpdated = DateTime.MinValue;
            this.LastUpdateSource = string.Empty;
            this.NumberOfSnoozes = source.NumberOfSnoozes;
            this.PauseTimeout = source.PauseTimeout;
            this.RestTime = source.RestTime;
            this.ShowClock = source.ShowClock;
            this.SnoozeTime = source.SnoozeTime;
            this.WorkTime = source.WorkTime;
        }

        public DBConfiguration(IConfiguration source)
        {
            this.ClockOpacity = source.ClockOpacity;
            this.LastUpdated = DateTime.MinValue;
            this.LastUpdateSource = string.Empty;
            this.NumberOfSnoozes = source.NumberOfSnoozes;
            this.PauseTimeout = source.PauseTimeout;
            this.RestTime = source.RestTime;
            this.ShowClock = source.ShowClock;
            this.SnoozeTime = source.SnoozeTime;
            this.WorkTime = source.WorkTime;
        }
    }
}
