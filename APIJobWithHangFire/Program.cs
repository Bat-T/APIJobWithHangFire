using APIJobWithHangFire.Services;
using Hangfire;
using Hangfire.Common;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Configure Redis connection
var redis = ConnectionMultiplexer.Connect("localhost:6379");

// Configure Hangfire to use Redis
builder.Services.AddHangfire(config =>
    config.UseRedisStorage(redis, new RedisStorageOptions
    {
        Prefix = "hangfire:"
    }));

builder.Services.AddHangfireServer();

// Add MyJobService to DI container
builder.Services.AddTransient<IJobService,JobService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Hangfire Dashboard (Optional)
app.UseHangfireDashboard();


//If we need to add any type of authorization
//app.UseHangfireDashboard("/hangfire", new DashboardOptions
//{
//    Authorization = new[] { new HangfireCustomAuthorizationFilter(app.Services.GetService<IHttpContextAccessor>()) }
//});


// Schedule a recurring job at a specific time every day
RecurringJob.AddOrUpdate<IJobService>(
    "daily-job",
    job => job.TriggerJob($"daily-job: Scheduler Job Ran at {DateTime.Now.ToString()}"),
    "18 16 * * *" // Cron expression to run the job every day at 4:18 pm
                  /*The cron expression "6 16 * * *" is structured as follows:
                   * 15 - The minute when the job should run(6th minute of the hour).
                   * 16 - The hour when the job should run(16th hour, which is 4 PM in a 24 - hour format).
                   * * -Any day of the month.
                   * * -Any month.
                   * * -Any day of the week.*/
);

app.Run();
