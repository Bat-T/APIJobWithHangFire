namespace APIJobWithHangFire.Services
{
    public class JobService : IJobService
    {
        public void TriggerJob(string messageWithTime)
        {
            Console.Write(messageWithTime);
        }
    }
}
