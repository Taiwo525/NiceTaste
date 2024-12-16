namespace LocalServicesMarketplace.Identity.API.Infrastructure.Configuration
{
    public class RedisConfig
    {
        public string ConnectionString { get; set; }
        public string InstanceName { get; set; }
        public int DefaultDatabase { get; set; }
        public bool AbortOnConnectFail { get; set; }
        public int ConnectRetry { get; set; }
        public int ConnectTimeout { get; set; }
        public bool AllowAdmin { get; set; }
    }
}
