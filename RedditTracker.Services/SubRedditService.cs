using RedditTracker.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Services
{
    public class SubRedditService : ISubRedditService
    {
        private readonly ISubRedditManagerService _subRedditManagerService;

        public SubRedditService(ISubRedditManagerService subRedditManagerService)
        {
            _subRedditManagerService = subRedditManagerService;
        }
        public async Task<string> GetTopPost(string subRedditName)
        {
            var subRedditState = await _subRedditManagerService.GetSubRedditState(subRedditName);
            var maxUpVotePost = subRedditState.PostVotes.Aggregate((max, current) => max.Value > current.Value ? max : current);
            return maxUpVotePost.Key;
        }

        public async Task<string> GetTopUser(string subRedditName)
        {
            var subRedditState = await _subRedditManagerService.GetSubRedditState(subRedditName);
            var maxPostUser = subRedditState.UserPosts.Aggregate((max, current) => max.Value > current.Value ? max : current);
            return maxPostUser.Key;
        }

        public async Task<DateTime> GetLastUpdateTime(string subRedditName)
        {
            var subRedditState = await _subRedditManagerService.GetSubRedditState(subRedditName);
            return subRedditState.LastFetchTime;
        }

        public async Task<bool> IsSubRedditTracked(string subRedditName)
        {
            return await _subRedditManagerService.HasSubReddit(subRedditName);
        }
    }
}
