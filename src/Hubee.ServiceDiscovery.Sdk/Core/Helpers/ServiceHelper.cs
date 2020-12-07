using Hubee.ServiceDiscovery.Sdk.Core.Models.Constants;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Hubee.ServiceDiscovery.Sdk.Core.Helpers
{
    internal static class ServiceHelper
    {
        public static int GetPort(IServer server)
        {
            var address = server.Features
                .Get<IServerAddressesFeature>()
                .Addresses
                .First();

            return new Uri(address).Port;
        }

        public static string GetNonLoopbackIp()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development")
                ? ServiceDefault.LOCAL_ADDRESS_IP
                : Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x =>
                        x.AddressFamily == AddressFamily.InterNetwork &&
                        x.ToString() != ServiceDefault.LOCAL_ADDRESS &&
                        x.ToString() != ServiceDefault.LOCAL_ADDRESS_IP)
                    .ToString();
        }
    }
}
