using Hubee.ServiceDiscovery.Sdk.Core.Helpers;
using System;

namespace Hubee.ServiceDiscovery.Sdk.Core.Models
{
    public class HubeeServiceDiscoveryConfig
    {
        public string ServiceName { get; set; }
        public string ServiceDiscovery { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
        public HealthCheckConfig HealthCheck { get; set; }
        public ServiceDiscoveryType ServiceDiscoveryType => EnumHelper.Parse<ServiceDiscoveryType>(this.ServiceDiscovery);

        public string GetAddress()
        {
            return $"http://{this.HostName}:{this.Port}";
        }

        public string GetInstanceRegistrationId()
        {
            return $"{this.ServiceName}-{Guid.NewGuid():N}";
        }

        public void CheckConfig()
        {
            var isInvalid = string.IsNullOrEmpty(this.ServiceName) ||
                 this.ServiceDiscoveryType.Equals(ServiceDiscoveryType.Undefined) ||
                 string.IsNullOrEmpty(this.HostName) ||
                 string.IsNullOrEmpty(this.Port) ||
                 this.HealthCheck is null ||
                 string.IsNullOrEmpty(this.HealthCheck.Endpoint) ||
                 this.HealthCheck.Interval <= 0 ||
                 this.HealthCheck.DeregisterCriticalServiceAfter <= 0;

            if (isInvalid)
                throw new InvalidOperationException($"Please, configure appsettings with a {nameof(HubeeServiceDiscoveryConfig)} section");
        }
    }
}