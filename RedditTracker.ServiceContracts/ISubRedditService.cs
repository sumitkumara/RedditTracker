namespace RedditTracker.ServiceContracts
{
    public interface ISubRedditService
    {
        Task<DateTime> GetLastUpdateTime(string subRedditName);
        Task<string> GetTopPost(string subRedditName);
        Task<string> GetTopUser(string subRedditName);
        Task<bool> IsSubRedditTracked(string subRedditName);
    }
}