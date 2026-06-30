---
uid: Codebelt.Extensions.AspNetCore.Text.Yaml
summary: *content
---

Integrate YamlDotNet serialization seamlessly into ASP.NET Core applications. This namespace provides extension methods and middleware to configure YAML formatter options and dependency injection for the `Codebelt.Extensions.YamlDotNet` package, enabling automatic YAML serialization for API responses and request handling.

[!INCLUDE [availability-default](../../includes/availability-default.md)]

Start with `AddYamlFormatter()` on `IServiceCollection` in your `Program.cs` to register the YAML formatter with its default configuration, or use `AddYamlFormatterOptions()` to customize serialization behavior such as converters, formatting style, and type mappings.

## When to use

- You're building an ASP.NET Core API that needs to serialize responses to YAML format.
- You want automatic YAML content negotiation without manually managing `YamlFormatter` instances.
- You need dependency injection for YAML configuration and customization across your application.
- You're integrating YamlDotNet with ASP.NET Core's content formatting pipeline.

## Related

- `Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml` — MVC-specific input/output formatters for detailed control over YAML serialization in request/response handling.
- `Codebelt.Extensions.YamlDotNet` — Core YAML serialization and formatter configuration.
- `Codebelt.Extensions.YamlDotNet.Converters` — Custom converters for specialized type serialization.

### Extension Members

| Type | Ext | Methods |
|------|-----|---------|
| `IServiceCollection` | ⬇️ | `AddYamlFormatter()`, `AddYamlFormatterOptions()`, `AddMinimalYamlOptions()` |
