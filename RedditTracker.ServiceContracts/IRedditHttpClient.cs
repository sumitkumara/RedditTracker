using RedditTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.ServiceContracts
{
    public interface IRedditHttpClient
    {
        Task<ApiResult<T>> DeleteAsync<T>(string url);
        Task<ApiResult<T>> GetAsync<T>(string url);
        Task<ApiResult<T>> GetAsync<T>(string route, IDictionary<string, string> parameters);
        Task<ApiResult<T>> PostAsJsonAsync<T>(string url, object content);
        Task<ApiResult<T>> PostAsync<T>(string url, string content = "");
        Task<ApiResult<T>> PutAsync<T>(string url);
        Task<ApiResult<T>> PutAsJsonAsync<T>(string url, object content);
    }
}
