using RedditTracker.Models;

namespace RedditTracker.ServiceContracts
{
    public interface ISubRedditManagerService
    {
        Task AddNewSubReddit(string subRedditName);
        Task<SubRedditState> GetSubRedditState(string subRedditName);
        Task<string> GetTopPost(string subRedditName);
        Task<bool> HasSubReddit(string subRedditName);
        Task UpdateSubRedditData(string subRedditName, SubRedditState subRedditState);
    }
}