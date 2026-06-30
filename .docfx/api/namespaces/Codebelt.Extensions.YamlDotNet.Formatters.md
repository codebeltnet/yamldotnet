---
uid: Codebelt.Extensions.YamlDotNet.Formatters
summary: *content
---
Serialize and deserialize objects using generic YAML serialization with a uniform API.

The `Codebelt.Extensions.YamlDotNet.Formatters` namespace exposes `YamlFormatter`, a generic stream-based formatter that handles YAML serialization and deserialization with configurable naming conventions, custom converters, and sensitivity settings. Use this namespace to work with YAML at the object level rather than low-level events.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

**Start with:** [YamlFormatter](xref:Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatter) to serialize objects, or [YamlFormatter.DeserializeObject](xref:Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatter.DeserializeObject*) to parse YAML with custom delegates.

**When to use:**
- Serialize .NET objects to YAML format with custom naming conventions
- Deserialize YAML streams using factory patterns
- Configure YAML formatting options globally or per-call

**Related:** [Codebelt.Extensions.YamlDotNet](xref:Codebelt.Extensions.YamlDotNet) · [Codebelt.Extensions.YamlDotNet.Converters](xref:Codebelt.Extensions.YamlDotNet.Converters)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|YamlFormatterOptions|⬇️|`SetPropertyName`|
