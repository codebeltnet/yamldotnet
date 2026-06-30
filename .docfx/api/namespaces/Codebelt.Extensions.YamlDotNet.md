---
uid: Codebelt.Extensions.YamlDotNet
summary: *content
---
Serialize and deserialize .NET objects to and from YAML with uniform APIs and flexible configuration.

The `Codebelt.Extensions.YamlDotNet` namespace provides the foundation for YamlDotNet-based YAML formatting across Codebelt, exposing generic serialization/deserialization APIs and low-level emitter helpers for building custom YAML converters.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

**Start with:** [YamlFormatter](xref:Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatter) to serialize and deserialize objects, or use [EmitterExtensions](xref:Codebelt.Extensions.YamlDotNet.EmitterExtensions) to write custom YAML structures.

**When to use:**
- Serialize .NET objects to YAML with configurable naming conventions and custom converters
- Build custom YAML converters for domain types
- Low-level YAML event writing when emitting complex structures

**Related:** [Codebelt.Extensions.YamlDotNet.Formatters](xref:Codebelt.Extensions.YamlDotNet.Formatters) · [Codebelt.Extensions.YamlDotNet.Converters](xref:Codebelt.Extensions.YamlDotNet.Converters)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IEmitter|⬇️|`WriteStartObject`, `WriteString`, `WritePropertyName`, `WriteValue`, `WriteEndObject`, `WriteStartArray`, `WriteEndArray`, `WriteObject`|
