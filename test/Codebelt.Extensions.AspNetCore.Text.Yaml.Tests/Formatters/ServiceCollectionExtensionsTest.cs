using System;
using System.Linq;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddYamlFormatterOptions_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((IServiceCollection)null).AddYamlFormatterOptions());
        }

        [Fact]
        public void AddYamlFormatterOptions_ShouldOnlyRegisterOptionsOnce()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddYamlFormatterOptions(o => o.SensitivityDetails = FaultSensitivityDetails.All);
            services.AddYamlFormatterOptions(o => o.SensitivityDetails = FaultSensitivityDetails.None);

            var count = services.Count(sd => sd.ServiceType == typeof(IConfigureOptions<YamlFormatterOptions>));

            TestOutput.WriteLine($"IConfigureOptions<YamlFormatterOptions> registration count: {count}");

            Assert.Equal(1, count);
        }

        [Fact]
        public void AddYamlExceptionResponseFormatter_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((IServiceCollection)null).AddYamlExceptionResponseFormatter());
        }

        [Fact]
        public void AddYamlExceptionResponseFormatter_ShouldOnlyRegisterFormatterOnce()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddYamlExceptionResponseFormatter();
            services.AddYamlExceptionResponseFormatter();

            var count = services.Count(sd => sd.ServiceType == typeof(HttpExceptionDescriptorResponseFormatter<YamlFormatterOptions>));

            TestOutput.WriteLine($"HttpExceptionDescriptorResponseFormatter<YamlFormatterOptions> registration count: {count}");

            Assert.Equal(1, count);
        }
    }
}
