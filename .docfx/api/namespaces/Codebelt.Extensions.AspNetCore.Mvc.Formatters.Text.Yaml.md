---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml
summary: *content
---
Add YAML input/output formatters to ASP.NET Core MVC applications.

The `Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml` namespace provides extension methods to register YAML input and output formatters with MVC. Use this namespace to enable YAML content negotiation in ASP.NET Core MVC controllers—both full framework (`IMvcBuilder`) and minimal APIs (`IMvcCoreBuilder`).

[!INCLUDE [availability-modern](../../includes/availability-modern.md)]

**Start with:** [MvcBuilderExtensions.AddYamlFormatters](xref:Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml.MvcBuilderExtensions.AddYamlFormatters*) to register YAML formatters with full MVC, or [MvcCoreBuilderExtensions.AddYamlFormatters](xref:Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml.MvcCoreBuilderExtensions.AddYamlFormatters*) for minimal APIs.

**When to use:**
- Add YAML content negotiation to ASP.NET Core MVC controllers (request/response bodies)
- Accept and return YAML from action methods with automatic serialization
- Configure YAML formatting for MVC responses (naming conventions, custom converters)

**Related:** [Codebelt.Extensions.AspNetCore.Text.Yaml](xref:Codebelt.Extensions.AspNetCore.Text.Yaml) · [Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters](xref:Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters)

### Extension Members

|Type|Ext|Methods|
|--:|:-:|---|
|IMvcBuilder|⬇️|`AddYamlFormatters`, `AddYamlFormattersOptions`|
|IMvcCoreBuilder|⬇️|`AddYamlFormatters`, `AddYamlFormattersOptions`|
