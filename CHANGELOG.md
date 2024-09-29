# Changelog

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

For more details, please refer to `PackageReleaseNotes.txt` on a per assembly basis in the `.nuget` folder.

> [!NOTE]  
> Changelog entries prior to version 8.4.0 was migrated from previous versions of `Cuemon.Extensions.YamlDotNet`, `Cuemon.Extensions.AspNetCore`, `Cuemon.Extensions.AspNetCore.Mvc` and `Cuemon.Extensions.Diagnostics`.

## [9.0.0] - TBD

This major release is first and foremost focused on ironing out any wrinkles that have been introduced with .NET 9 preview releases so the final release is production ready together with the official launch from Microsoft.

## [8.4.0] - 2024-09-20

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
