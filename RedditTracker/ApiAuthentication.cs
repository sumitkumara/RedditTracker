using RedditTracker.Models.Configurations;

namespace RedditTracker
{
    public class ApiAuthetication
    {
        private readonly RequestDelegate _next;
        private readonly IAppSettings _appSettings;

        public ApiAuthetication(RequestDelegate next, IAppSettings appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("/swagger"))
            {
                await _next.Invoke(context);
                return;
            }
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                if (authHeader != $"Basic {_appSettings.ApiKey}")
                {
                    //Unauthorized
                    context.Response.StatusCode = 401;
                    return;
                }


                await _next.Invoke(context);
            }
            else
            {
                // no authorization header - Unauthorized
                context.Response.StatusCode = 401;
                return;
            }
        }
    }
}
