using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hubee.ServiceDiscovery.Sdk.Core.Interfaces
{
    internal interface IServiceDiscovery
    {
        IServiceCollection Register(IServiceCollection services, IConfiguration configuration);
    }
}
