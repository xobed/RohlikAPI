using StackExchange.Redis;

namespace RohlikAPIWeb.Model
{
    public class AppSettings
    {
        public string REDIS_URL { get; set; }
        public string DATADOG_APIKEY { get; set; }

        public ConfigurationOptions GetRedisSettings()
        {
            REDIS_URL = REDIS_URL.Replace("redis://", "");
            var split = REDIS_URL.Split("@");

            if (split.Length == 2)
            {
                var usernamePassword = split[0].Split(":");
                var hostPort = split[1];

                return new ConfigurationOptions
                {
                    EndPoints = {hostPort},
                    Password = usernamePassword[1]
                };
            }
            else
            {
                var hostPort = split[0];
                return new ConfigurationOptions
                {
                    EndPoints = {hostPort}
                };
            }
        }
    }
}