using System;
using Consul;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Hubee.ServiceDiscovery.Sdk.Core.Models;
using Hubee.ServiceDiscovery.Sdk.Core.Helpers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Hubee.ServiceDiscovery.Sdk.Infra.Consul.Services
{
    internal class RegistrationService : IHostedService
    {
        private CancellationTokenSource _stoppingCancellationTokenSource;
        private readonly IConsulClient _consulClient;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IServer _server;
        private readonly IOptions<HubeeServiceDiscoveryConfig> _config;
        private string _instanceRegistrationId;
        private readonly ILogger<RegistrationService> _logger;
        private readonly IApplicationLifetime _applicationLifetime;

        public RegistrationService(
            IConsulClient consulClient,
            IHostingEnvironment hostingEnvironment,
            IServer server,
            IOptions<HubeeServiceDiscoveryConfig> config,
            ILogger<RegistrationService> logger,
            IApplicationLifetime applicationLifetime
            )
        {
            _consulClient = consulClient;
            _hostingEnvironment = hostingEnvironment;
            _server = server;
            _config = config;
            _logger = logger;
            _applicationLifetime = applicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _applicationLifetime.ApplicationStarted.Register(async () =>
                {
                    _stoppingCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    var servicePort = ServiceHelper.GetPort(_server);
                    var serviceIp = ServiceHelper.GetNonLoopbackIp();

                    _instanceRegistrationId = _config.Value.GetInstanceRegistrationId();

                    var registration = new AgentServiceRegistration
                    {
                        ID = _instanceRegistrationId,
                        Name = _config.Value.ServiceName,
                        Address = serviceIp,
                        Port = servicePort,
                        Check = new AgentServiceCheck
                        {
                            HTTP = $"http://{serviceIp}:{servicePort}/{_config.Value.HealthCheck.Endpoint}",
                            Interval = TimeSpan.FromSeconds(_config.Value.HealthCheck.Interval),
                            DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(_config.Value.HealthCheck.DeregisterCriticalServiceAfter),
                            CheckID = _instanceRegistrationId
                        }
                    };

                    await _consulClient.Agent.ServiceDeregister(registration.ID, _stoppingCancellationTokenSource.Token);
                    await _consulClient.Agent.ServiceRegister(registration, _stoppingCancellationTokenSource.Token);

                    RegisterConsulInstanceId();
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service discovery: register service ({_config.Value.ServiceName}) failed", ex.StackTrace);
                throw ex;
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Deregistering {_instanceRegistrationId}");
                _stoppingCancellationTokenSource.Cancel();
                await _consulClient.Agent.ServiceDeregister(_instanceRegistrationId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Service discovery: deregisteration service ({_config.Value.ServiceName}) failed", ex.StackTrace);
                throw ex;
            }
        }

        private void RegisterConsulInstanceId()
        {
            File.WriteAllText(Path.Combine(_hostingEnvironment.ContentRootPath, @"App.properties"),
                $"consul.instance-id={_instanceRegistrationId}");
        }
    }
}
