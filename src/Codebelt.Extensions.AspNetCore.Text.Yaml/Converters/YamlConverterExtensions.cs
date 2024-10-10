using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cuemon.Diagnostics;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Converters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon;
using Cuemon.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YamlDotNet.Core;

namespace Codebelt.Extensions.AspNetCore.Text.Yaml.Converters
{
    /// <summary>
    /// Extension methods for the <see cref="YamlConverter"/> class.
    /// </summary>
    public static class YamlConverterExtensions
    {
        /// <summary>
        /// Adds a <see cref="ProblemDetails"/> YAML converter to the collection.
        /// </summary>
        /// <param name="converters">The collection of <see cref="YamlConverter"/> to extend.</param> 
        /// <returns>A reference to <paramref name="converters"/> so that additional calls can be chained.</returns>
        public static ICollection<YamlConverter> AddProblemDetailsConverter(this ICollection<YamlConverter> converters)
        {
            converters.Add(YamlConverterFactory.Create<ProblemDetails>(WriteProblemDetails));
            converters.Add(YamlConverterFactory.Create<IDecorator<ProblemDetails>>((writer, dpd, formatter) => WriteProblemDetails(writer, dpd.Inner, formatter)));
            return converters;
        }

        private static void WriteProblemDetails(IEmitter writer, ProblemDetails pd, YamlFormatter formatter)
        {
            writer.WriteStartObject();
            if (pd.Type != null) { writer.WriteString(formatter.Options.SetPropertyName(nameof(ProblemDetails.Type)), pd.Type); }
            if (pd.Title != null) { writer.WriteString(formatter.Options.SetPropertyName(nameof(ProblemDetails.Title)), pd.Title); }
            if (pd.Status.HasValue) { writer.WriteString(formatter.Options.SetPropertyName(nameof(ProblemDetails.Status)), formatter.Options.Settings.Formatter.FormatNumber(pd.Status)); }
            if (pd.Detail != null) { writer.WriteString(formatter.Options.SetPropertyName(nameof(ProblemDetails.Detail)), pd.Detail); }
            if (pd.Instance != null) { writer.WriteString(formatter.Options.SetPropertyName(nameof(ProblemDetails.Instance)), pd.Instance); }

            foreach (var extension in pd.Extensions.Where(kvp => kvp.Value != null))
            {
                writer.WritePropertyName(formatter.Options.SetPropertyName(extension.Key));
                writer.WriteObject(extension.Value, formatter.Options);
            }

            writer.WriteEndObject();
        }

        /// <summary>
        /// Adds an <see cref="HttpExceptionDescriptor"/> YAML converter to the list.
        /// </summary>
        /// <param name="converters">The <see cref="T:ICollection{YamlConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<YamlConverter> AddHttpExceptionDescriptorConverter(this ICollection<YamlConverter> converters, Action<ExceptionDescriptorOptions> setup = null)
        {
            var converter = YamlConverterFactory.Create<HttpExceptionDescriptor>(type => type == typeof(HttpExceptionDescriptor), (writer, value, formatter) =>
            {
                Validator.ThrowIfInvalidConfigurator(setup, out var options);

                writer.WriteStartObject();
                writer.WritePropertyName(formatter.Options.SetPropertyName("Error"));

                writer.WriteStartObject();
                if (value.Instance != null)
                {
                    writer.WritePropertyName(formatter.Options.SetPropertyName("Instance"));
                    writer.WriteValue(value.Instance.OriginalString);
                }
                writer.WriteString(formatter.Options.SetPropertyName("Status"), value.StatusCode.ToString(CultureInfo.InvariantCulture));
                writer.WriteString(formatter.Options.SetPropertyName("Code"), value.Code);
                writer.WriteString(formatter.Options.SetPropertyName("Message"), value.Message);
                if (value.HelpLink != null)
                {
                    writer.WriteString(formatter.Options.SetPropertyName("HelpLink"), value.HelpLink.OriginalString);
                }
                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure))
                {
                    writer.WritePropertyName(formatter.Options.SetPropertyName("Failure"));
                    new ExceptionConverter(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data))
                    {
                        Formatter = formatter
                    }.WriteYaml(writer, value.Failure);
                }
                writer.WriteEndObject();

                if (options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) && value.Evidence.Any())
                {
                    writer.WritePropertyName(formatter.Options.SetPropertyName("Evidence"));
                    writer.WriteStartObject();
                    foreach (var evidence in value.Evidence)
                    {
                        writer.WritePropertyName(formatter.Options.SetPropertyName(evidence.Key));
                        writer.WriteObject(evidence.Value, formatter.Options);
                    }
                    writer.WriteEndObject();
                }

                if (!string.IsNullOrWhiteSpace(value.CorrelationId))
                {
                    writer.WriteString(formatter.Options.SetPropertyName(nameof(value.CorrelationId)), value.CorrelationId);
                }
                if (!string.IsNullOrWhiteSpace(value.RequestId))
                {
                    writer.WriteString(formatter.Options.SetPropertyName(nameof(value.RequestId)), value.RequestId);
                }

                if (!string.IsNullOrWhiteSpace(value.TraceId))
                {
                    writer.WriteString(formatter.Options.SetPropertyName(nameof(value.TraceId)), value.TraceId);
                }

                writer.WriteEndObject();
            });

            if (!converters.Any(c => c.CanConvert(typeof(HttpExceptionDescriptor)))) { converters.Add(converter); }
            return converters;
        }
    }
}
