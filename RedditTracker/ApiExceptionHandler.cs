using System.Net;

namespace RedditTracker
{
    public class ApiExceptionHandler
    {
        private readonly RequestDelegate _next;

        //private ILoggingHttpClient _loggingClient { get; }
        //public IAppSettings AppSettings { get; }

        public ApiExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogError(ex, context);
                await HandleExceptionAsync(context, ex);
                //if (AppSettings.Environment == "Development")
                //    throw;
            }
        }

        private async Task LogError(Exception exception, HttpContext context)
        {

        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // not awaited to let this execute asynchronously 
            LogError(exception, context);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        private string GetInnermostExceptionMessage(Exception exception)
        {
            if (exception.InnerException != null)
                return GetInnermostExceptionMessage(exception.InnerException);

            return exception.Message;
        }
    }
}
