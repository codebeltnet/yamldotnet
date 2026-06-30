---
uid: Codebelt.Extensions.YamlDotNet.Converters
summary: *content
---
Create custom YAML converters for domain types and diagnostic objects.

The `Codebelt.Extensions.YamlDotNet.Converters` namespace provides abstract `YamlConverter` base classes and factory methods for building type-specific YAML serializers. Use this namespace to handle specialized serialization logic for diagnostics (like `Failure` exceptions) and other domain types that need custom YAML representation.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

**Start with:** [YamlConverter](xref:Codebelt.Extensions.YamlDotNet.Converters.YamlConverter) base class to build custom converters, or use [YamlConverterFactory](xref:Codebelt.Extensions.YamlDotNet.Converters.YamlConverterFactory) to create converters with delegates.

**When to use:**
- Create custom converters for domain types that need special YAML formatting
- Register exception and failure converters in the YAML formatter
- Build converters that wrap low-level `IEmitter` events into readable YAML structures

**Related:** [Codebelt.Extensions.YamlDotNet](xref:Codebelt.Extensions.YamlDotNet) · [Codebelt.Extensions.YamlDotNet.Formatters](xref:Codebelt.Extensions.YamlDotNet.Formatters)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection&lt;YamlConverter&gt;|⬇️|`AddFailureConverter`|
