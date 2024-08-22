namespace APIJobWithHangFire.Services
{
    public class JobService : IJobService
    {
        public void TriggerJob(string messageWithTime)
        {
            Console.Write($"\n Timestamp {DateTime.Now.ToLongDateString()} : {DateTime.Now.ToLongTimeString()} -- {messageWithTime}");
        }
    }
}
