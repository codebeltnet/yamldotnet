---
uid: Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters.ServiceCollectionExtensions
example:
- *content
---

This example demonstrates how to configure YAML formatter options and add an exception response formatter to an ASP.NET Core application using service collection extensions. It shows the setup workflow: creating a web application builder, registering YAML formatter options with custom sensitivity settings through `AddYamlFormatterOptions`, adding an exception response formatter through `AddYamlExceptionResponseFormatter` with different sensitivity settings, and then building the app with a route that throws an exception to test the error handling. The observable outcome is that the application is configured to handle exceptions by serializing them as YAML responses with the specified detail levels.

```csharp
using System;
using Codebelt.Extensions.AspNetCore.Text.Yaml.Formatters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        // Add YAML formatter options with custom sensitivity settings
        builder.Services.AddYamlFormatterOptions(options =>
        {
            options.SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace | FaultSensitivityDetails.FailureWithData;
        });
        
        // Add exception response formatter using YAML
        builder.Services.AddYamlExceptionResponseFormatter(options =>
        {
            options.SensitivityDetails = FaultSensitivityDetails.All;
        });
        
        var app = builder.Build();
        
        app.MapGet("/error", (HttpContext context) => 
        {
            throw new InvalidOperationException("Example exception");
        });
        
        app.MapGet("/", async (HttpContext context) =>
        {
            await context.Response.WriteAsync("Hello World");
        });
        
        app.Run();
    }
}
```
