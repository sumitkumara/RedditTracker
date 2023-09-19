using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Configurations
{
    public interface ILogSettings
    {
        string LogPath { get; }
        int NumberOfDaysToKeepBackup { get; }
        int PerformanceAlertTime { get; }
    }
}
