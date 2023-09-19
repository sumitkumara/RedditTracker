using RedditTracker.Models;

namespace RedditTracker.ServiceContracts
{
    public interface IRedditAuthHttpClient
    {
        Task<ApiResult<T>> PostFormUrlEncoded<T>(string url, IEnumerable<KeyValuePair<string, string>> postData);
    }
}