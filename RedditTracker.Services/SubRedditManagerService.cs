using RedditTracker.Models;
using RedditTracker.ServiceContracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Services
{
    public class SubRedditManagerService: ISubRedditManagerService
    {
        ConcurrentDictionary<string, SubRedditState> SubRedditManager = new();

        public async Task AddNewSubReddit(string subRedditName)
        {
            SubRedditManager[subRedditName] = new SubRedditState();
        }

        public async Task UpdateSubRedditData(string subRedditName, SubRedditState subRedditState)
        {
            SubRedditManager[subRedditName] = subRedditState;
            LimitInMemoryDataStructures(SubRedditManager[subRedditName]);
        }

        public async Task<string> GetTopPost(string subRedditName)
        {
            var result = SubRedditManager.TryGetValue(subRedditName, out var subRedditState);

            if (!result)
                return string.Empty;

             var maxUpVotePost = subRedditState.PostVotes.Aggregate((max, current) => max.Value > current.Value ? max : current);

            return maxUpVotePost.Key;
        }

        public async Task<bool> HasSubReddit(string subRedditName) => SubRedditManager.ContainsKey(subRedditName);
        public async Task<SubRedditState> GetSubRedditState(string subRedditName) => SubRedditManager[subRedditName];

        // to take care of in-memory limit
        private void LimitInMemoryDataStructures(SubRedditState subRedditState)
        {
            if (subRedditState.PostVotes.Count > 1000)
            {
                // Order the dictionary by value in descending order
                var orderedDictionary = subRedditState.PostVotes.OrderByDescending(kv => kv.Value).ToList();
                // Take the top N entries to keep
                var entriesToKeep = orderedDictionary.Take(500);
                subRedditState.PostVotes = entriesToKeep.ToDictionary(kv => kv.Key, kv => kv.Value);
            }

            if (subRedditState.UserPosts.Count > 1000)
            {
                var orderedDictionary = subRedditState.UserPosts.OrderByDescending(kv => kv.Value).ToList();
                var entriesToKeep = orderedDictionary.Take(500);
                subRedditState.UserPosts = entriesToKeep.ToDictionary(kv => kv.Key, kv => kv.Value);
            }

        }
    }
}
