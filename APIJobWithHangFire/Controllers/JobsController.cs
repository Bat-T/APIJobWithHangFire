using APIJobWithHangFire.Helper;
using APIJobWithHangFire.Services;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIJobWithHangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IJobService _jobService;

        public JobsController(IRecurringJobManager recurringJobManager, IJobService jobService)
        {
            _recurringJobManager = recurringJobManager;
            _jobService = jobService;
        }

        // Endpoint to manually trigger the recurring job
        [HttpPost("trigger-recurring-now")]
        public IActionResult TriggerRecurringNow()
        {
            _recurringJobManager.Trigger("daily-job");

            //_recurringJobManager.AddOrUpdate(
            //"every-2-seconds-job",
            //() => _jobService.TriggerJob(DateTime.Now.ToString()),
            //CustomCron.EveryNSeconds(2) // Runs every 2 seconds
            //);


            return Ok("Recurring job triggered now!");
        }


        [HttpPost("schedule-recurring-every-2-seconds")]
        public IActionResult ScheduleRecurringEvery2Seconds()
        {
            _recurringJobManager.AddOrUpdate(
                "every-2-seconds-job",
                () => _jobService.TriggerJob($"reccuring-2-seconds-job: Scheduler Job Ran at {DateTime.Now.ToString()}"),
                "50 4 * * *" // Runs every 2 seconds
            );
            return Ok("Recurring job scheduled to run every 2 seconds!");
        }

        // Endpoint to remove the recurring job
        [HttpPost("remove-daily-recurring")]
        public IActionResult RemoveRecurring()
        {
            _recurringJobManager.RemoveIfExists("daily-job");
            return Ok("Recurring daily-job removed!");
        }

        // Endpoint to remove the recurring job
        [HttpPost("remove-2-seconds-recurring")]
        public IActionResult Remove2SecondRecurring()
        {
            _recurringJobManager.RemoveIfExists("every-2-seconds-job");
            return Ok("every-2-seconds job  removed!");
        }
    }
}
