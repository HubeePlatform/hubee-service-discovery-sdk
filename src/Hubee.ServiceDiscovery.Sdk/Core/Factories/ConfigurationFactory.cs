using Hubee.ServiceDiscovery.Sdk.Core.Exceptions;
using Hubee.ServiceDiscovery.Sdk.Core.Interfaces;
using Hubee.ServiceDiscovery.Sdk.Core.Models;
using Hubee.ServiceDiscovery.Sdk.Infra.Consul;

namespace Hubee.ServiceDiscovery.Sdk.Core.Factories
{
    internal class ConfigurationFactory
    {
        public IServiceDiscovery GetByConfig(HubeeServiceDiscoveryConfig config)
        {
            return config.ServiceDiscoveryType switch
            {
                ServiceDiscoveryType.Consul => new ConsulServiceDiscovery(config),
                _ => throw new ServiceDiscoveryNotSupportedException(config.ServiceDiscovery)
            };
        }
    }
}

