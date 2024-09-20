using System;
using System.Linq;
using Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions;
using Cuemon.Extensions.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml
{
    public class ServiceProviderExtensionsTest : Test
    {
        public ServiceProviderExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetExceptionResponseFormatters_ShouldGetAllRegisteredServicesOf_IExceptionResponseFormatter() // # legacy test from  Cuemon.Extensions.AspNetCore.Diagnostics
        {
            var services = new ServiceCollection();

            services.AddOptions();
            services.AddYamlExceptionResponseFormatter();

            var serviceProvider = services.BuildServiceProvider();

            var formatters = serviceProvider.GetExceptionResponseFormatters().ToList();

            var formattersAndResponseHandlers = formatters.SelectMany(formatter => formatter.ExceptionDescriptorHandlers.Select(handler => $"{formatter.GetType().GenericTypeArguments[0].Name} -> {handler.ContentType}")).ToList();

            TestOutput.WriteLine(formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));

            Assert.Equal(5, formattersAndResponseHandlers.Count);
            Assert.Equal("""
                         YamlFormatterOptions -> text/plain; charset=utf-8
                         YamlFormatterOptions -> text/plain
                         YamlFormatterOptions -> application/yaml
                         YamlFormatterOptions -> text/yaml
                         YamlFormatterOptions -> */*
                         """.ReplaceLineEndings(), formattersAndResponseHandlers.ToDelimitedString(o => o.Delimiter = Environment.NewLine));
        }
    }
}
