using Hubee.ServiceDiscovery.Sdk.Core.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Hubee.ServiceDiscovery.Sdk.Tests.Core
{
    public class TestBase
    {
        private static IConfiguration GetConfiguration(string nameSetting, string path = "")
        {
            return new ConfigurationBuilder()
                  .SetBasePath($"{Directory.GetCurrentDirectory()}\\Core\\Settings\\{path}")
                  .AddJsonFile($"{nameSetting}.json")
                  .Build();
        }

        public HubeeServiceDiscoveryConfig GetConfig(string nameSetting, string path = "")
        {
            var configuration = GetConfiguration(nameSetting, path);
            var configServiceDiscovery = new HubeeServiceDiscoveryConfig();

            configuration.GetSection("HubeeServiceDiscoveryConfig").Bind(configServiceDiscovery);

            return configServiceDiscovery;
        }
    }
}
