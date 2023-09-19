using RedditTracker.Models;
using RedditTracker.Models.Configurations;
using RedditTracker.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Services
{
    public class RedditAuthHttpClient : IRedditAuthHttpClient
    {
        private readonly HttpClient _httpClient;

        public RedditAuthHttpClient(HttpClient httpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://www.reddit.com/api/v1/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {appSettings.RedditAppAuthToken}");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "subRTracker");
        }

        public async Task<ApiResult<T>> PostFormUrlEncoded<T>(string url, IEnumerable<KeyValuePair<string, string>> postData)
        {
            using (var content = new FormUrlEncodedContent(postData))
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var responseMessage = await _httpClient.PostAsync(url, content);

                return await HandleResponseResult<T>(responseMessage);
            }
        }

        private async Task<ApiResult<T>> HandleResponseResult<T>(HttpResponseMessage responseMessage)
        {
            var result = new ApiResult<T> { StatusCode = responseMessage.StatusCode };

            try
            {
                if (responseMessage.IsSuccessStatusCode)
                {
                    result.Content = await responseMessage.Content.ReadAsAsync<T>();
                    result.Location = responseMessage.Headers.Location?.ToString();
                    return result;
                }

                result.Error = true;
                result.ErrorMessage = await responseMessage.Content.ReadAsStringAsync();
                // uncomment when client can read log configuration
                //_logger.WriteErrorBase(new Exception(result.ErrorMessage));
                result.Content = default;
            }
            catch (Exception excep)
            {
                result.Error = true;
                result.ErrorMessage = excep.Message;
                // uncomment when client can read log configuration
                //_logger.WriteErrorBase(excep);
                throw;
            }

            return result;

        }
    }
}
