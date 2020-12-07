namespace Hubee.ServiceDiscovery.Sdk.Core.Models
{
    public class HealthCheckConfig
    {
        public string Endpoint { get; set; }
        public int Interval { get; set; }
        public int DeregisterCriticalServiceAfter { get; set; }
    }
}