using System;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Xunit;
using YamlDotNet.Serialization.NamingConventions;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml.Converters
{
    public class YamlConverterExtensionsTest : Test
    {
        public YamlConverterExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddHttpExceptionDescriptorConverter_ShouldSerializeHttpExceptionDescriptorToYamlFormat()
        {
            var sut = new HttpExceptionDescriptor(new InvalidOperationException("Detailed failure."), 404, "NotFound", "Resource missing.", new Uri("https://docs.example.com/errors/not-found"))
            {
                Instance = new Uri("https://api.example.com/resources/42"),
                CorrelationId = "corr-123",
                RequestId = "req-456",
                TraceId = "trace-789"
            };
            var formatter = new YamlFormatter(o =>
            {
                o.Settings.NamingConvention = PascalCaseNamingConvention.Instance;
                o.Settings.Converters.AddHttpExceptionDescriptorConverter(edo => edo.SensitivityDetails = FaultSensitivityDetails.All);
            });
            var result = formatter.Serialize(sut).ToEncodedString();

            TestOutput.WriteLine(result);

            Assert.StartsWith("""
                             Error:
                               Instance: https://api.example.com/resources/42
                               Status: 404
                               Code: NotFound
                               Message: Resource missing.
                               HelpLink: https://docs.example.com/errors/not-found
                               Failure:
                                 Type: System.InvalidOperationException
                             """.ReplaceLineEndings(), result);
            Assert.DoesNotContain("Evidence:", result);
            Assert.Contains("CorrelationId: corr-123".ReplaceLineEndings(), result);
            Assert.Contains("RequestId: req-456".ReplaceLineEndings(), result);
            Assert.Contains("TraceId: trace-789".ReplaceLineEndings(), result);
        }
    }
}
