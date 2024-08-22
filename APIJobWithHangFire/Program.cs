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
builder.Services.AddTransient<IRecurringJobService, RecurringJobService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<LongRunningTask>();

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
RecurringJob.AddOrUpdate<IRecurringJobService>(
    "interval-job1",
    job => job.AddOrUpdateJob(), $"0 {DateTime.Now.Hour}/1 * * *",new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local}
);

app.Run();
