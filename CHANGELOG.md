# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

> [!NOTE]  
> Changelog entries prior to version 8.4.0 was migrated from previous versions of `Cuemon.Extensions.YamlDotNet`, `Cuemon.Extensions.AspNetCore`, `Cuemon.Extensions.AspNetCore.Mvc` and `Cuemon.Extensions.Diagnostics`.

## [10.0.2] - 2026-02-15

This is a service update that focuses on package dependencies.

## [10.0.1] - 2026-01-22

This is a service update that focuses on package dependencies.

## [10.0.0] - 2025-11-13

This is a major release that focuses on adapting the latest `.NET 10` release (LTS) in exchange for current `.NET 8` (LTS).

> To ensure access to current features, improvements, and security updates, and to keep the codebase clean and easy to maintain, we target only the latest long-term (LTS), short-term (STS) and (where applicable) cross-platform .NET versions.

## [9.0.8] - 2025-10-20

This is a service update that focuses on package dependencies.

## [9.0.7] - 2025-09-15

This is a service update that focuses on package dependencies.

## [9.0.6] - 2025-08-16

This is a service update that focuses on package dependencies.

## [9.0.5] - 2025-07-11

This is a service update that focuses on package dependencies.

## [9.0.4] - 2025-06-15

This is a service update that focuses on package dependencies.

## [9.0.3] - 2025-05-24

This is a service update that focuses on package dependencies.

## [9.0.2] - 2025-04-15

This is a service update that focuses on package dependencies.

## [9.0.1] - 2025-01-28

This is a service update that primarily focuses on package dependencies and minor improvements.

## [9.0.0] - 2024-11-13

This major release is first and foremost focused on ironing out any wrinkles that have been introduced with .NET 9 preview releases so the final release is production ready together with the official launch from Microsoft.

### Added

- YamlConverterExtensions class in the Codebelt.Extensions.YamlDotNet.Converters namespace that consist of one extension method for the ICollection{YamlConverter} interface: `AddFailureConverter`
- YamlConverterExtensions class in the Codebelt.Extensions.AspNetCore.Text.Yaml.Converters namespace received a new extension method for the IServiceCollection interface: `AddProblemDetailsConverter`

### Changed

- YamlSerializerOptions class in the Codebelt.Extensions.YamlDotNet namespace to use `camelCase` naming convention instead of `PascalCase` and omit null values by default
- ToYaml method on the ExceptionDescriptorExtensions class in the Codebelt.Extensions.YamlDotNet.Diagnostics namespace to have a signature of `Action{YamlFormatterOptions}` (breaking change)
- YamlConverterFactory class in the Codebelt.Extensions.YamlDotNet namespace to include a `YamlFormatter` parameter in all factory methods (breaking change)
- YamlConverter class in the Codebelt.Extensions.YamlDotNet.Converters namespace to expose a YamlFormatter property (`Formatter`) instead of a YamlFormatterOptions (FormatterOptions) property (breaking change)
- AddHttpExceptionDescriptorConverter method on the YamlConverterExtensions class in the Codebelt.Extensions.AspNetCore.Text.Yaml.Converters namespace to support `Instance` (Uri) and `TraceId` (string)
- AddYamlExceptionResponseFormatter method on the ServiceCollectionExtensions class in the Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters namespace to support `ProblemDetails`

### Fixed

- AddYamlFormattersOptions method on the MvcBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace to pass setup delegate to AddYamlExceptionResponseFormatter
- AddYamlFormattersOptions method on the MvcCoreBuilderExtensions class in the Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml namespace to pass setup delegate to AddYamlExceptionResponseFormatter

## [8.4.0] - 2024-09-20

### Added

ExceptionDescriptorExtensions class in the Codebelt.Extensions.YamlDotNet.Diagnostics namespace that converts an ExceptionDescriptor to YAML

### Changed

- YamlFormatter class in the Codebelt.Extensions.YamlDotNet.Formatters namespace to now supports excluding non-essential properties from serialization
- YamlFormatter class in the Codebelt.Extensions.YamlDotNet.Formatters namespace now provides full control of YAML deserialization using an action delegate factory (IDeserializer, Parser)

## [8.3.2] - 2024-08-04

### Fixed

- YamlFormatter class in the Codebelt.Extensions.YamlDotNet.Formatters namespace to use WithCaseInsensitivePropertyMatching (https://github.com/aaubry/YamlDotNet/discussions/946)
  - Although v16.0.0 of YamlDotNet has breaking changes, this is not reflected in the API from Codebelt.Extensions.YamlDotNet until next major release


## [8.3.0] - 2024-04-09

### Added

- ExceptionConverter class in the Codebelt.Extensions.YamlDotNet.Converters namespace that converts an Exception to YAML
- ExceptionDescriptorConverter class in the Codebelt.Extensions.YamlDotNet.Converters namespace that converts an ExceptionDescriptor to YAML
- YamlConverter class in the Codebelt.Extensions.YamlDotNet.Converters namespace that converts an object to and from YAML (YAML ain't markup language)
- YamlFormatter class in the Codebelt.Extensions.YamlDotNet.Formatters namespace that serializes and deserializes an object, in YAML format
- YamlFormatterOptions class in the Codebelt.Extensions.YamlDotNet.Formatters namespace that provides configuration options for YamlFormatter
- YamlFormatterOptionsExtensions class in the Codebelt.Extensions.YamlDotNet.Formatters namespace that consist of one extension method for the YamlFormatterOptions class: SetPropertyName
- EmitterExtensions class in the Codebelt.Extensions.YamlDotNet namespace that consist of many extension method for the IEmitter interface: WriteStartObject, WriteString, WritePropertyName, WriteValue, WriteEndObject, WriteStartArray, WriteEndArray and WriteObject
- NodeOptions class in the Codebelt.Extensions.YamlDotNet namespace that provides configuration options for EmitterExtensions
- ScalarOptions class in the Codebelt.Extensions.YamlDotNet namespace that provides configuration options for EmitterExtensions
- YamlConverterFactory class in the Codebelt.Extensions.YamlDotNet namespace that provides a factory based way to create and wrap an YamlConverter implementations
- YamlSerializerOptions class in the Codebelt.Extensions.YamlDotNet namespace that provides configuration options for SerializerBuilder and DeserializerBuilder
