using System;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon;

namespace Codebelt.Extensions.YamlDotNet.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="ExceptionDescriptor"/> class.
    /// </summary>
    public static class ExceptionDescriptorExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="descriptor"/> to its equivalent string representation.
        /// </summary>
        /// <param name="descriptor">The <see cref="ExceptionDescriptor"/> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A string that represents the specified <paramref name="descriptor"/>.</returns>
        public static string ToYaml(this ExceptionDescriptor descriptor, Action<YamlFormatterOptions> setup = null)
        {
            Validator.ThrowIfNull(descriptor);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var formatter = new YamlFormatter(options);
            return formatter.Serialize(descriptor).ToEncodedString();
        }
    }
}
