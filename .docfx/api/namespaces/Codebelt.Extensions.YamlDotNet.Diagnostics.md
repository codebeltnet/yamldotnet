---
uid: Codebelt.Extensions.YamlDotNet.Diagnostics
summary: *content
---
Convert exception and failure diagnostics to YAML format.

The `Codebelt.Extensions.YamlDotNet.Diagnostics` namespace provides extension methods to serialize exception descriptors into YAML. Use this namespace when you need to format diagnostic information (exceptions, failures, stack traces) as human-readable YAML for logging or response bodies.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

**Start with:** [ExceptionDescriptor.ToYaml](xref:Codebelt.Extensions.YamlDotNet.Diagnostics.ExceptionDescriptorExtensions.ToYaml*) to convert exception descriptors to YAML strings.

**When to use:**
- Format exception diagnostics as YAML for structured logging
- Convert `ExceptionDescriptor` objects to YAML in error handlers
- Serialize failure information for error responses or diagnostics endpoints

**Related:** [Codebelt.Extensions.YamlDotNet.Formatters](xref:Codebelt.Extensions.YamlDotNet.Formatters) · [Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ExceptionDescriptor|⬇️|`ToYaml`|
