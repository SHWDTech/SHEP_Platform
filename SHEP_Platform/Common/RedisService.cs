using StackExchange.Redis;

namespace SHEP_Platform.Common
{
    public class RedisService
    {
        private static readonly IDatabase RedisDatabase;

        static RedisService()
        {
            RedisDatabase = ConnectionMultiplexer.Connect("localhost").GetDatabase();
        }

        public static IDatabase GetRedisDatabase() => RedisDatabase;
    }
}