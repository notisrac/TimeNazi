using GDataDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNazi
{
    public class BaseConfiguration : IConfiguration
    {
        public int WorkTime { get; set; }
        public int SnoozeTime { get; set; }
        public int RestTime { get; set; }
        public bool ShowClock { get; set; }
        public int ClockOpacity { get; set; }
        public int NumberOfSnoozes { get; set; }
        public int PauseTimeout { get; set; }
        [GDataDBSkip]
        public static BaseConfiguration Default { get { return new BaseConfiguration() { ClockOpacity = 50, NumberOfSnoozes = 1, PauseTimeout = 10, RestTime = 10, ShowClock = true, SnoozeTime = 10, WorkTime = 50 }; } }
    }
}
