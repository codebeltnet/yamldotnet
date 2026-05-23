using System;
using System.IO;
using Codebelt.Extensions.Xunit;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Extensions.IO;
using Xunit;
using YamlDotNet.Core;

namespace Codebelt.Extensions.YamlDotNet.Converters
{
    public class ExceptionConverterTest : Test
    {
        public ExceptionConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ReadYaml_ShouldThrowNotImplementedException()
        {
            var converter = new ExceptionConverter();
            var parser = new Parser(new StringReader("message: test\n"));

            Assert.Throws<NotImplementedException>(() => converter.ReadYaml(parser, typeof(Exception)));
        }

        [Fact]
        public void WriteYaml_ShouldSerializeExceptionWithInnerException()
        {
            var inner = new InvalidOperationException("Inner error");
            var outer = new InvalidOperationException("Outer error", inner);

            var formatter = new YamlFormatter(o =>
            {
                o.Settings.Converters.Add(new ExceptionConverter());
            });

            var yaml = formatter.Serialize(outer, typeof(InvalidOperationException)).ToEncodedString();
            TestOutput.WriteLine(yaml);

            Assert.Contains("Outer error", yaml);
            Assert.Contains("Inner error", yaml);
        }

        [Fact]
        public void WriteYaml_ShouldSerializeAggregateException()
        {
            var inner1 = new InvalidOperationException("Inner error 1");
            var inner2 = new ArgumentException("Inner error 2");
            var agg = new AggregateException("Aggregate error", inner1, inner2);

            var formatter = new YamlFormatter(o =>
            {
                o.Settings.Converters.Add(new ExceptionConverter());
            });

            var yaml = formatter.Serialize(agg, typeof(AggregateException)).ToEncodedString();
            TestOutput.WriteLine(yaml);

            Assert.Contains("Inner error 1", yaml);
            Assert.Contains("Inner error 2", yaml);
        }
    }
}
