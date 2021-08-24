using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hubee.ServiceDiscovery.Sdk.Core.Interfaces
{
    public interface IDnsServiceDiscovery
    {
        Task<string> SearchByNameAsync(string serviceName);
    }
}
