using System;
using Codebelt.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;

namespace Codebelt.Extensions.YamlDotNet
{
    public class YamlSerializerOptionsTest : Test
    {
        public YamlSerializerOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenConvertersIsNull()
        {
            var options = new YamlSerializerOptions
            {
                Converters = null
            };

            var ex = Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
            Assert.Contains("Converters == null", ex.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldThrow_WhenReflectionRulesIsNull()
        {
            var options = new YamlSerializerOptions
            {
                ReflectionRules = null
            };

            var ex = Assert.Throws<InvalidOperationException>(() => options.ValidateOptions());
            Assert.Contains("ReflectionRules == null", ex.Message);
        }

        [Fact]
        public void ValidateOptions_ShouldNotThrow_WhenOptionsAreValid()
        {
            var options = new YamlSerializerOptions();

            var ex = Record.Exception(() => options.ValidateOptions());

            Assert.Null(ex);
        }
    }
}
