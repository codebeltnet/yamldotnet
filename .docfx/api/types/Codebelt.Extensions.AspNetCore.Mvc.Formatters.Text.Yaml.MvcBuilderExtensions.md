---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml.MvcBuilderExtensions
example:
- *content
---

This example configures ASP.NET Core's MVC builder with YAML input and output formatters using the builder extension pattern. It shows how to register formatters during startup with custom sensitivity settings that control whether stack traces, data, or evidence are included in serialized exceptions. The example demonstrates both the fluent `AddYamlFormatters` method (with inline options) and the separate `AddYamlFormattersOptions` method, illustrating how to build the host and configure the container for YAML request/response handling in MVC applications.

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
        
        // Add MVC with YAML formatters
        builder.Services.AddControllersWithViews()
            .AddYamlFormatters(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
            });
        
        // Or configure options separately
        builder.Services.AddControllersWithViews()
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
