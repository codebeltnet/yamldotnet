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
        /// <param name="converters">The <see cref="ICollection{YamlConverter}" /> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to <paramref name="converters"/> after the operation has completed.</returns>
        public static ICollection<YamlConverter> AddHttpExceptionDescriptorConverter(this ICollection<YamlConverter> converters, Action<ExceptionDescriptorOptions> setup = null)
        {
            var converter = YamlConverterFactory.Create<HttpExceptionDescriptor>(type => type == typeof(HttpExceptionDescriptor), (writer, value, formatter) => WriteHttpExceptionDescriptor(writer, value, formatter, setup));

            if (!converters.Any(c => c.CanConvert(typeof(HttpExceptionDescriptor)))) { converters.Add(converter); }
            return converters;
        }

        private static void WriteHttpExceptionDescriptor(IEmitter writer, HttpExceptionDescriptor value, YamlFormatter formatter, Action<ExceptionDescriptorOptions> setup)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);

            writer.WriteStartObject();
            writer.WritePropertyName(formatter.Options.SetPropertyName("Error"));

            WriteError(writer, value, formatter, options);
            WriteEvidence(writer, value, formatter, options);
            WriteOptionalString(writer, formatter, nameof(value.CorrelationId), value.CorrelationId);
            WriteOptionalString(writer, formatter, nameof(value.RequestId), value.RequestId);
            WriteOptionalString(writer, formatter, nameof(value.TraceId), value.TraceId);

            writer.WriteEndObject();
        }

        private static void WriteError(IEmitter writer, HttpExceptionDescriptor value, YamlFormatter formatter, ExceptionDescriptorOptions options)
        {
            writer.WriteStartObject();
            WriteOptionalUri(writer, formatter, "Instance", value.Instance);
            writer.WriteString(formatter.Options.SetPropertyName("Status"), value.StatusCode.ToString(CultureInfo.InvariantCulture));
            writer.WriteString(formatter.Options.SetPropertyName("Code"), value.Code);
            writer.WriteString(formatter.Options.SetPropertyName("Message"), value.Message);
            WriteOptionalUri(writer, formatter, "HelpLink", value.HelpLink);
            WriteFailure(writer, value, formatter, options);
            writer.WriteEndObject();
        }

        private static void WriteFailure(IEmitter writer, HttpExceptionDescriptor value, YamlFormatter formatter, ExceptionDescriptorOptions options)
        {
            if (!options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Failure)) { return; }

            writer.WritePropertyName(formatter.Options.SetPropertyName("Failure"));
            new ExceptionConverter(options.SensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Data))
            {
                Formatter = formatter
            }.WriteYaml(writer, value.Failure);
        }

        private static void WriteEvidence(IEmitter writer, HttpExceptionDescriptor value, YamlFormatter formatter, ExceptionDescriptorOptions options)
        {
            if (!options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence) || !value.Evidence.Any()) { return; }

            writer.WritePropertyName(formatter.Options.SetPropertyName("Evidence"));
            writer.WriteStartObject();
            foreach (var evidence in value.Evidence)
            {
                writer.WritePropertyName(formatter.Options.SetPropertyName(evidence.Key));
                writer.WriteObject(evidence.Value, formatter.Options);
            }
            writer.WriteEndObject();
        }

        private static void WriteOptionalString(IEmitter writer, YamlFormatter formatter, string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) { return; }

            writer.WriteString(formatter.Options.SetPropertyName(propertyName), value);
        }

        private static void WriteOptionalUri(IEmitter writer, YamlFormatter formatter, string propertyName, Uri value)
        {
            if (value == null) { return; }

            writer.WriteString(formatter.Options.SetPropertyName(propertyName), value.OriginalString);
        }
    }
}
