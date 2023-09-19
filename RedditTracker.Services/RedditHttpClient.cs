using Newtonsoft.Json;
using RedditTracker.Models;
using RedditTracker.Models.Configurations;
using RedditTracker.Models.Responses;
using RedditTracker.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Services
{
    public class RedditHttpClient: IRedditHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IRedditAuthHttpClient _redditAuthHttpClient;
        private readonly IAppSettings _appSettings;

        public RedditHttpClient(HttpClient httpClient, IRedditAuthHttpClient redditAuthHttpClient, IAppSettings appSettings)
        {
            _httpClient = httpClient;
            _redditAuthHttpClient = redditAuthHttpClient;
            _appSettings = appSettings;
            _httpClient.BaseAddress = new Uri("https://oauth.reddit.com/r/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "subRTracker");
        }

        public Task<ApiResult<T>> DeleteAsync<T>(string url)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<T>> GetAsync<T>(string url)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url)
            {
                //Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            await Authenticate(requestMessage);
            var responseMessage = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

            return await HandleResponseResult<T>(responseMessage);
        }

        public Task<ApiResult<T>> GetAsync<T>(string route, IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<T>> PostAsJsonAsync<T>(string url, object content)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<T>> PostAsync<T>(string url, string content = "")
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<T>> PutAsJsonAsync<T>(string url, object content)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<T>> PutAsync<T>(string url)
        {
            throw new NotImplementedException();
        }

        private async Task<ApiResult<T>> HandleResponseResult<T>(HttpResponseMessage responseMessage)
        {
            var result = new ApiResult<T> { StatusCode = responseMessage.StatusCode };

            try
            {
                foreach (var header in responseMessage.Headers)
                {
                    result.Headers[header.Key] = string.Join(",", header.Value);
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    result.Content = await responseMessage.Content.ReadAsAsync<T>();
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

       
        private async Task Authenticate(HttpRequestMessage requestMessage)
        {
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("grant_type", "password"));
            collection.Add(new("username", _appSettings.RedditUserName));
            collection.Add(new("password", _appSettings.RedditPassword));
            var authResponse = await _redditAuthHttpClient.PostFormUrlEncoded<OAuthResponse>("access_token", collection);
            if (authResponse.Error)
            {
                // log and do something
                return;
            }

            var token = authResponse.Content.Access_Token;
            requestMessage.Headers.Add("Authorization", $"Bearer {token}");
        }

    }
}
