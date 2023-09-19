namespace RedditTracker.ServiceContracts
{
    public interface ISqlService
    {
        Task ExecuteScalarAsync(string command);
    }
}