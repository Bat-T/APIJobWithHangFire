namespace APIJobWithHangFire.Services
{
    public interface IRecurringJobService
    {
        void AddOrUpdateJob();
        void ExecuteJob();
        void RemoveJob();
    }
}