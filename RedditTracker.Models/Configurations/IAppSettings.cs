using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Configurations
{
    public interface IAppSettings
    {
        string RedditUserName { get;  }
        string RedditPassword { get;  }
        string RedditAppAuthToken { get; }
        string ApiKey { get; }
    }
}
