using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Configurations
{
    public class LogSettings : ILogSettings
    {
        public string LogPath { get; set; }
        public int NumberOfDaysToKeepBackup { get; set; }
        public int PerformanceAlertTime { get; set; }
    }
}
