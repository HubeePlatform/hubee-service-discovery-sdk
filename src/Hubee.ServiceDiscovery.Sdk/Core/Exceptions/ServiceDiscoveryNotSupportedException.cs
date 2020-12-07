using System;

namespace Hubee.ServiceDiscovery.Sdk.Core.Exceptions
{
    internal class ServiceDiscoveryNotSupportedException : Exception
    {
        public ServiceDiscoveryNotSupportedException(string typeName) : base($"Service Discovery {typeName} not supported")
        {

        }
    }
}