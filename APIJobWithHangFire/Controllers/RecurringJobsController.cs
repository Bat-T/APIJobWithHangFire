using APIJobWithHangFire.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIJobWithHangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurringJobsController : ControllerBase
    {
        private readonly IRecurringJobService _recurringjobService;

        public RecurringJobsController(IRecurringJobService recurringjobService)
        {
            _recurringjobService = recurringjobService;
        }

        [HttpPost("start-job")]
        public IActionResult StartJob()
        {
            _recurringjobService.AddOrUpdateJob();
            return Ok("Job scheduled!");
        }

        [HttpPost("stop-job")]
        public IActionResult StopJob()
        {
            _recurringjobService.RemoveJob();
            return Ok("Job removed!");
        }
    }
}
