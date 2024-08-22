namespace APIJobWithHangFire.Services
{
    public interface IJobService
    {
        public void TriggerJob(string messageWithTime);
    }
}
