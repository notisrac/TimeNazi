using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeNazi
{
    public class ActivityLogEntry
    {
        public DateTime LogDate { get; set; }
        public ActivityType Activity { get; set; }
        public long ElapsedTime { get; set; } // only set at xxxEnd events
        public string Source { get; set; } // Environment.MachineName
        public string UserName { get; set; } // Environment.UserName

        public override string ToString()
        {
            return string.Format("\"{0}\",\"{1}\",{2},\"{3}\",\"{4}\"", LogDate, Activity, ElapsedTime, Source, UserName);
        }
    }
}
