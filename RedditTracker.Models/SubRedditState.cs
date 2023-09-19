using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models
{
    public class SubRedditState
    {
        public string NextPageLink { get; set; }
        public DateTime LastFetchTime { get; set; }
        public string SubRedditName { get; set; }
        public Dictionary<string, long> UserPosts { get; set; } = new Dictionary<string, long>();
        public Dictionary<string, long> PostVotes { get; set; } = new Dictionary<string, long>();
        public string TopPost { get; set; }
        public string LastPostId { get; set; }
    }
}
