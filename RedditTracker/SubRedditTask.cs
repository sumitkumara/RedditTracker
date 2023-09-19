using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RedditTracker.Models;
using RedditTracker.Models.Responses;
using RedditTracker.ServiceContracts;

namespace RedditTracker
{
    public class SubRedditTask : PollingTask
    {
        private readonly IRedditHttpClient _redditHttpClient;
        private readonly ISubRedditManagerService _subRedditManagerService;
        public SubRedditTask(string name, IRedditHttpClient redditHttpClient, ISubRedditManagerService subRedditManagerService) : base(TimeSpan.FromSeconds(5)) // this interval could go in appsettings
        {
            Name = name;
            _redditHttpClient = redditHttpClient;
            _subRedditManagerService = subRedditManagerService;
            _subRedditManagerService.AddNewSubReddit(Name);
        }

        protected override async Task DoTask()
        {
            var subRedditState = await _subRedditManagerService.GetSubRedditState(Name);
            var url = $"{Name}/new/";

            if (!string.IsNullOrEmpty(subRedditState.NextPageLink))
            {
                url += $"?before={subRedditState.NextPageLink}";
            }
            //else if (subRedditState.LastFetchTime > DateTime.MinValue)
            //    return;// since no new posts

            var response = await _redditHttpClient.GetAsync<dynamic>(url);
            if (response.Error)
                return;

            var data = response.Content?.data;
            subRedditState.NextPageLink = data.after;

            SubRedditChildElement postsData = JsonConvert.DeserializeObject<SubRedditChildElement>(JsonConvert.SerializeObject(data));

            subRedditState.LastFetchTime = DateTime.UtcNow;
            foreach (var post in postsData.Children)
            {
                // already scanned these posts
                if (subRedditState.LastPostId == post.Data.Id)
                    break;
                subRedditState.NextPageLink = data.after;
                subRedditState.UserPosts[post.Data.Author] = subRedditState.UserPosts.TryGetValue(post.Data.Author, out long existingValue)
                ? existingValue + 1 : 1;
                subRedditState.PostVotes[post.Data.Id] = post.Data.Ups;
            }

            subRedditState.LastPostId = postsData.Children.First().Data.Id;
            await _subRedditManagerService.UpdateSubRedditData(Name, subRedditState);

            // Decide next api call time based on header values

            var rateLimitUsed = response.Headers["X-Ratelimit-Used"];
            var rateLimitRemaining = response.Headers["X-Ratelimit-Remaining"];
            var rateLimitReset = response.Headers["X-Ratelimit-Reset"];

            // Check if we are close to hitting the rate limit
            if (int.TryParse(rateLimitRemaining, out int remaining) && remaining <= 5)
            {
                // If close to hitting the rate limit, calculate the delay until reset and wait
                if (int.TryParse(rateLimitReset, out int resetTime))
                {
                    var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    var delaySeconds = Math.Max(0, resetTime - currentTime + 1); // Add an extra second as a buffer
                    Interval = TimeSpan.FromSeconds(delaySeconds);//Delaying until rate limit resets
                }
            }
            else // keep normal polling interval
            {
                Interval = TimeSpan.FromSeconds(5);
            }
        }

    }

    public class SubRedditChildElement
    {
        public List<SubRedditPostData> Children { get; set; }
    }
    public class SubRedditPostData
    {
        public string Kind { get; set; }
        public SubRedditResponse Data { get; set; }
    }
}

