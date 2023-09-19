using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Responses
{
    public class OAuthResponse
    {
        public string Access_Token { get; set; }
        public string Token_Type { get; set; }
        public long Expires_In { get; set; }
        public string Scope { get; set; }
    }
}
