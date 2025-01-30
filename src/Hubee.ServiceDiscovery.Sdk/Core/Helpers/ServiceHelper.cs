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
                .FirstOrDefault();

            if (address == null)
            {
                throw new InvalidOperationException("Server started, but address not found");
            }

            if (address.Contains("+:"))
                return int.Parse(address.Split(':')[2]);

            return new Uri(address).Port;
        }

        public static string GetNonLoopbackIp()
        {
            return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(x =>
                        x.AddressFamily == AddressFamily.InterNetwork).ToString();
        }
    }
}
