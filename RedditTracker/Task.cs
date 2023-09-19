namespace RedditTracker
{
    public abstract class BaseTask
    {
        protected BaseTask()
        {
        }

        protected virtual string Name { get; set; }

        public virtual void Start() { }

        public virtual void Stop() { }
    }

    public abstract class PollingTask : BaseTask
    {
        public TimeSpan Interval { get; protected set; }
        protected PollingTask(TimeSpan interval)
        {
            Interval = interval;
        }

        protected abstract Task DoTask();

        public sealed override async void Start()
        {
            do
            {
                try
                {
                    await DoTask();
                }
                catch (Exception exception)
                {
                    // log error
                }

                await Task.Delay(Interval);

            } while (true);
        }
    }
    public abstract class PipelineTask : PollingTask
    {
        protected PipelineTask(TimeSpan interval) : base(interval)
        {

        }
    }
}
