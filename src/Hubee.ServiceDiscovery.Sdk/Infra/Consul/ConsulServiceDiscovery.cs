﻿using Consul;
using DnsClient;
using Hubee.ServiceDiscovery.Sdk.Core.Interfaces;
using Hubee.ServiceDiscovery.Sdk.Core.Models;
using Hubee.ServiceDiscovery.Sdk.Infra.Consul.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace Hubee.ServiceDiscovery.Sdk.Infra.Consul
{
    internal class ConsulServiceDiscovery : IServiceDiscovery
    {
        public ConsulServiceDiscovery(HubeeServiceDiscoveryConfig config)
        {
            Config = config;
        }

        public HubeeServiceDiscoveryConfig Config { get; set; }


        public IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HubeeServiceDiscoveryConfig>(configuration.GetSection("HubeeServiceDiscoveryConfig"));

            services.AddSingleton<IHostedService, RegistrationService>();
            services.AddScoped<IDnsServiceDiscovery, DnsServiceDiscovery>();

            if (!string.IsNullOrEmpty(Config.DnsIpAddress))
            {
                services.AddSingleton<IDnsQuery>(p =>
                {
                    return new LookupClient(IPAddress.Parse(Config.DnsIpAddress), Config.DnsPort);
                });
            }

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri(this.Config.GetAddress());
            }));

            return services;
        }
    }
}
