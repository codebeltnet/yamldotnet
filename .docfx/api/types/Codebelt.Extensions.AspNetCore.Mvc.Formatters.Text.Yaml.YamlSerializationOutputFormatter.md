---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml.YamlSerializationOutputFormatter
example:
- *content
---

The following example shows how to register and verify the `YamlSerializationOutputFormatter` in an ASP.NET Core MVC application that serializes controller results to YAML when clients request YAML format via the Accept header.

```csharp
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace ExampleNamespace;

public class Item
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [HttpGet("item")]
    public IActionResult GetItem()
    {
        var item = new Item { Id = 1, Name = "Example Item", Description = "Sample data" };
        return Ok(item);
    }
}

class Program
{
    static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        
        // Register YAML formatters - this adds YamlSerializationOutputFormatter
        builder.Services.AddControllers()
            .AddYamlFormatters();
        
        var app = builder.Build();
        
        // Verify the formatter is registered in MVC options
        var mvcOptions = app.Services.GetRequiredService<IOptions<MvcOptions>>();
        var outputFormatters = mvcOptions.Value.OutputFormatters;
        var yamlOutputFormatter = outputFormatters.OfType<YamlSerializationOutputFormatter>().FirstOrDefault();
        
        if (yamlOutputFormatter != null)
        {
            Console.WriteLine($"YamlSerializationOutputFormatter registered for types: {string.Join(", ", yamlOutputFormatter.SupportedMediaTypes)}");
        }
        
        app.MapControllers();
        app.Run();
    }
}
```
