---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml.MvcCoreBuilderExtensions
example:
- *content
---

This example configures ASP.NET Core's lightweight MVC core builder with YAML input and output formatters using the builder extension pattern. It shows how to register formatters during startup when using `AddMvcCore()` for minimal APIs or custom configurations, including custom sensitivity settings to control exception serialization detail levels. The example demonstrates both the fluent `AddYamlFormatters` method (with inline options) and the separate `AddYamlFormattersOptions` method, illustrating the setup workflow and how both approaches integrate with the container and request/response pipeline.

```csharp
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        // Add MVC core services with YAML formatters
        var mvcCoreBuilder = builder.Services.AddMvcCore()
            .AddYamlFormatters(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
            });
        
        // Or configure options separately with required parameter
        builder.Services.AddMvcCore()
            .AddYamlFormattersOptions(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.All;
            });
        
        var app = builder.Build();
        
        app.MapControllers();
        
        app.Run();
    }
}
```
