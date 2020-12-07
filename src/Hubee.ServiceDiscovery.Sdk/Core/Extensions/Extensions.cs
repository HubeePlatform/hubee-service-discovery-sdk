using Hubee.ServiceDiscovery.Sdk.Core.Factories;
using Hubee.ServiceDiscovery.Sdk.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hubee.ServiceDiscovery.Sdk.Core.Extensions
{
    public static class Extensions
    {
        private static readonly ConfigurationFactory _configurationFactory = new ConfigurationFactory();

        public static IServiceCollection AddServiceDiscovery(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new HubeeServiceDiscoveryConfig();
            configuration.GetSection("HubeeServiceDiscoveryConfig").Bind(config);

            config.CheckConfig();

            var serviceDiscovery = _configurationFactory.GetByConfig(config);
            serviceDiscovery.Register(services, configuration);

            return services;
        }
    }
}