using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models.Responses
{
    public class SubRedditResponse
    {
        public long Ups { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Id { get; set; }
    }
}
