using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedditTracker.Models
{
    public class ApiResult
    {
        public string ErrorMessage { get; set; }
        public bool Error { get; set; }
        public bool Success => !Error;
        public HttpStatusCode StatusCode { get; set; }
        public string Location { get; set; }
        public string Etag { get; set; }
        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();    
    }

    public class ApiResult<T> : ApiResult
    {
        public T Content { get; set; }
    }
}
