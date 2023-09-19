using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Configurations
{
    public class AppSettings: IAppSettings
    {
        public string RedditUserName { get; set; }
        public string RedditPassword { get; set; }
        public string RedditAppAuthToken { get; set; }

        public string ApiKey { get; set; }
    }
}
