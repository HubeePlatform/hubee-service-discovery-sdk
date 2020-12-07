using System;
using Xunit;

namespace Hubee.ServiceDiscovery.Sdk.Tests.Core
{
    public class ConfigurationTest: TestBase
    {

        [Theory]
        [InlineData("invalid_consul")]
        [InlineData("invalid_service_name")]
        [InlineData("invalid_healthcheck")]
        [InlineData("invalid_healthcheck_endpoint")]
        [InlineData("invalid_healthcheck_interval")]
        public void Should_DoNotAcceptSettings_When_Invalid(string nameSetting)
        {
            var config = GetConfig(nameSetting, "Invalid");
            Assert.Throws<InvalidOperationException>(() => config.CheckConfig());
        }

        [Theory]
        [InlineData("valid_consul")]
        public void Should_AcceptSettings_When_valid(string nameSetting)
        {
            var config = GetConfig(nameSetting, "Valid");

            config.CheckConfig();

            Assert.True(true);
        }

    }
}
