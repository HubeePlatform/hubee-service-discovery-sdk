using DnsClient;
using Hubee.ServiceDiscovery.Sdk.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hubee.ServiceDiscovery.Sdk.Infra.Consul.Services
{
    public class DnsServiceDiscovery : IDnsServiceDiscovery
    {
        private readonly IDnsQuery _dnsQuery;

        public DnsServiceDiscovery(IDnsQuery dnsQuery)
        {
            _dnsQuery = dnsQuery;
        }

        public async Task<string> SearchByNameAsync(string serviceName)
        {
            var result = await _dnsQuery.ResolveServiceAsync(serviceName);

            if (result is null || result.Length <= 0)
                return null;

            var record = result[0];

            if (record.AddressList?.Length <= 0)
                return null;

            return $"http://{record.AddressList[0]}:{record.Port}";
        }
    }
}
