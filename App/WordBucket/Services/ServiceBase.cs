namespace WordBucket.Services
{
    public class ServiceBase
    {
        public bool IsInitialize { get; private set; }

        public virtual async Task InitializeAsync()
        {
            IsInitialize = true;
            await Task.CompletedTask;
        }
    }
}
