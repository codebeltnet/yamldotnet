---
uid: Codebelt.Extensions.AspNetCore.Text.Yaml.Converters.YamlConverterExtensions
example:
- *content
---

The following example demonstrates how to register converters for ASP.NET Core exception types with a YAML formatter.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Codebelt.Extensions.AspNetCore.Text.Yaml.Converters;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Converters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        var converters = new List<YamlConverter>();
        
        // Register the ProblemDetails converter
        converters.AddProblemDetailsConverter();
        
        // Register the HttpExceptionDescriptor converter with custom sensitivity settings
        converters.AddHttpExceptionDescriptorConverter(options =>
        {
            options.SensitivityDetails = FaultSensitivityDetails.StackTrace | FaultSensitivityDetails.Data;
        });
        
        var formatter = new YamlFormatter(new YamlFormatterOptions { Settings = new YamlSerializerOptions { Converters = converters } });
        
        var problemDetails = new ProblemDetails
        {
            Type = "https://example.com/errors/validation",
            Title = "Validation Error",
            Status = 400,
            Detail = "One or more validation errors occurred."
        };
        
        var yaml = formatter.Serialize(problemDetails, typeof(ProblemDetails));
        using (var reader = new StreamReader(yaml))
        {
            Console.WriteLine(reader.ReadToEnd());
        }
    }
}
```
