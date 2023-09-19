using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Configurations
{
    public interface IConnectionStrings
    {
        string DefaultConnection { get; }
    }
}
