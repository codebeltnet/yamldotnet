using System;
using System.Linq;
using Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddMinimalYamlOptions_ShouldThrowArgumentNullException_WhenServicesIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => ((IServiceCollection)null).AddMinimalYamlOptions());
        }

        [Fact]
        public void AddMinimalYamlOptions_ShouldRegisterYamlExceptionResponseFormatter()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddMinimalYamlOptions();

            var count = services.Count(sd => sd.ServiceType == typeof(HttpExceptionDescriptorResponseFormatter<YamlFormatterOptions>));

            TestOutput.WriteLine($"HttpExceptionDescriptorResponseFormatter<YamlFormatterOptions> registration count: {count}");

            Assert.Equal(1, count);
        }
    }
}
