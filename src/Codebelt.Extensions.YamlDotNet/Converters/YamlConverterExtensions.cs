using System.Collections.Generic;
using Cuemon.Diagnostics;

namespace Codebelt.Extensions.YamlDotNet.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="YamlConverter"/> class.
    /// </summary>
    public static class YamlConverterExtensions
    {
        /// <summary>
        /// Adds a <see cref="Failure"/> YAML converter to the collection.
        /// </summary>
        /// <param name="converters">The collection of <see cref="YamlConverter"/> to extend.</param> 
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        public static ICollection<YamlConverter> AddFailureConverter(this ICollection<YamlConverter> converters)
        {
            converters.Add(YamlConverterFactory.Create<Failure>((writer, failure, formatter) =>
            {
                new ExceptionConverter(formatter.Options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), formatter.Options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data))
                {
                    Formatter = formatter
                }.WriteYaml(writer, failure.GetUnderlyingException());
            }));
            return converters;
        }
    }
}
