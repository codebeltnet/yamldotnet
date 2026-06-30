---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml.YamlSerializationInputFormatter
example:
- *content
---

The following example shows how to register and verify the `YamlSerializationInputFormatter` in an ASP.NET Core MVC application that accepts YAML-formatted request bodies and deserializes them to model objects.

```csharp
using System;
using System.Linq;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExampleNamespace;

public class CreateItemRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [HttpPost("item")]
    public IActionResult CreateItem([FromBody] CreateItemRequest request)
    {
        return Ok(new { message = $"Item '{request.Name}' created successfully" });
    }
}

class Program
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        // Register YAML formatters - this adds YamlSerializationInputFormatter
        builder.Services.AddControllers()
            .AddYamlFormatters();
        
        var app = builder.Build();
        
        // Verify the formatter is registered in MVC options
        var mvcOptions = app.Services.GetRequiredService<IOptions<MvcOptions>>();
        var inputFormatters = mvcOptions.Value.InputFormatters;
        var yamlInputFormatter = inputFormatters.OfType<YamlSerializationInputFormatter>().FirstOrDefault();
        
        if (yamlInputFormatter != null)
        {
            Console.WriteLine($"YamlSerializationInputFormatter registered for types: {string.Join(", ", yamlInputFormatter.SupportedMediaTypes)}");
        }
        
        app.MapControllers();
        app.Run();
    }
}
```
