---
uid: Codebelt.Extensions.AspNetCore.Text.Yaml.Converters
summary: *content
---
Add YAML converters for ASP.NET Core types like ProblemDetails and HttpExceptionDescriptor.

The `Codebelt.Extensions.AspNetCore.Text.Yaml.Converters` namespace provides extension methods to register YAML converters for ASP.NET Core types. Use this namespace to customize how ProblemDetails, HTTP exceptions, and exception descriptors are serialized to YAML in error responses.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

**Start with:** [YamlConverterExtensions.AddProblemDetailsConverter](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Converters.YamlConverterExtensions.AddProblemDetailsConverter*) or [YamlConverterExtensions.AddHttpExceptionDescriptorConverter](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Converters.YamlConverterExtensions.AddHttpExceptionDescriptorConverter*) to register converters for exception handling.

**When to use:**
- Customize YAML serialization of `ProblemDetails` in API error responses
- Format HTTP exception descriptors with sensitivity controls for stack traces and diagnostic data
- Build chains of custom converters for domain-specific error types

**Related:** [Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters) · [Codebelt.Extensions.YamlDotNet.Converters](xref:Codebelt.Extensions.YamlDotNet.Converters)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|ICollection&lt;YamlConverter&gt;|⬇️|`AddProblemDetailsConverter`, `AddHttpExceptionDescriptorConverter`|
