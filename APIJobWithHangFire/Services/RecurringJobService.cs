namespace APIJobWithHangFire.Services
{
    using Hangfire;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;

    public class RecurringJobService : IRecurringJobService
    {
        private readonly ILogger<RecurringJobService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly LongRunningTask _longRunningTask;

        public RecurringJobService(ILogger<RecurringJobService> logger, IConfiguration configuration, IRecurringJobManager recurringJobManager,LongRunningTask longRunningTask)
        {
            _logger = logger;
            _configuration = configuration;
            _recurringJobManager = recurringJobManager;
            _longRunningTask = longRunningTask;
        }

        public void AddOrUpdateJob()
        {
            var startHour = _configuration.GetValue<int>("JobSettings:StartHour");
            var startMinute = _configuration.GetValue<int>("JobSettings:StartMinute");
            var intervalInHours = _configuration.GetValue<int>("JobSettings:IntervalInHours");

            var initialDelay = TimeSpan.FromHours((24 - (DateTime.UtcNow.Hour - startHour) + intervalInHours) % intervalInHours);
            var cronExpression = $"0 {DateTime.UtcNow.Hour}/1 * * *";

            _recurringJobManager.AddOrUpdate(
                "interval-job",
                () => ExecuteJob(),
                cronExpression
            );

            _logger.LogInformation($"Job scheduled to run at {startHour}:{startMinute} UTC every {intervalInHours} hours.");
        }

        public void RemoveJob()
        {
            _recurringJobManager.RemoveIfExists("interval-job");
            _logger.LogInformation("Recurring job removed.");
        }

        public void ExecuteJob()
        {
            _logger.LogInformation($"Job executed at {DateTime.UtcNow}");
            // Implement your job logic here
            _longRunningTask.AddValueinRedis($"Recurring Job Ran at - {DateTime.Now.ToLongDateString().ToString()}: {DateTime.Now.ToLongTimeString().ToString()}").GetAwaiter().GetResult();
            Console.WriteLine($"Recurring Job Ran at - {DateTime.Now.ToLongDateString().ToString()}: {DateTime.Now.ToLongTimeString().ToString()}");
        }
    }
}
