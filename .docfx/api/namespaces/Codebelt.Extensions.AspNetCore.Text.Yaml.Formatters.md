---
uid: Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters
summary: *content
---
Configure YAML formatting options and register exception response formatters in ASP.NET Core.

The `Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters` namespace provides service collection extensions to configure `YamlFormatterOptions` and register HTTP exception response formatters that serialize errors to YAML. Use this namespace to customize YAML formatting behavior (naming conventions, sensitivity levels) and handle error responses.

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

**Start with:** [ServiceCollectionExtensions.AddYamlFormatterOptions](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters.ServiceCollectionExtensions.AddYamlFormatterOptions*) to configure YAML formatting, or [ServiceCollectionExtensions.AddYamlExceptionResponseFormatter](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters.ServiceCollectionExtensions.AddYamlExceptionResponseFormatter*) to handle error responses.

**When to use:**
- Configure YAML formatter options (naming conventions, custom converters, sensitivity details) for the application
- Register YAML-based exception/failure formatters for HTTP error responses
- Customize how exceptions, problem details, and diagnostic information appear in YAML responses

**Related:** [Codebelt.Extensions.AspNetCore.Text.Yaml](xref:Codebelt.Extensions.AspNetCore.Text.Yaml) · [Codebelt.Extensions.AspNetCore.Text.Yaml.Converters](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Converters) · [Codebelt.Extensions.YamlDotNet.Formatters](xref:Codebelt.Extensions.YamlDotNet.Formatters)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IServiceCollection|⬇️|`AddYamlFormatterOptions`, `AddYamlExceptionResponseFormatter`|
