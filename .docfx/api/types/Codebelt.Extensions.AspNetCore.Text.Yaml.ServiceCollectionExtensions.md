---
uid: Codebelt.Extensions.AspNetCore.Text.Yaml.ServiceCollectionExtensions
example:
- *content
---

This example demonstrates how to register minimal YAML formatting options in an ASP.NET Core application using dependency injection. It shows the setup workflow: creating a web application builder, registering YAML options through the `AddMinimalYamlOptions` method with custom sensitivity settings that control exception serialization detail levels, building the application, and defining routes. The observable outcome is that the application is configured to use YAML serialization with the specified exception sensitivity settings for request/response handling.

```csharp
using System.Threading.Tasks;
using Codebelt.Extensions.AspNetCore.Text.Yaml;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleNamespace;

class Program
{
    static async Task Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        // Add YAML formatting with default options and exception response formatter
        builder.Services.AddMinimalYamlOptions(options =>
        {
            // Customize YAML serialization and exception sensitivity details
            options.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
        });
        
        var app = builder.Build();
        
        app.MapGet("/", () => "YAML is now registered!");
        
        await app.RunAsync();
    }
}
```
