using StackExchange.Redis;

namespace APIJobWithHangFire.Services
{
    public class LongRunningTask
    {
        public async Task<string> AddValueinRedis(string valueToAdd)
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            var db = redis.GetDatabase();

            db.StringSet("testKey", valueToAdd);
            var value = await db.StringGetAsync("testKey");

            return value.ToString();
        }

    }
}
