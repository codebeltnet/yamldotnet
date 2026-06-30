---
uid: Codebelt.Extensions.YamlDotNet.Converters.YamlConverterExtensions
example:
- *content
---

The following example demonstrates how to register a converter for the `Failure` type with a YAML formatter.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Converters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Diagnostics;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        var converters = new List<YamlConverter>();
        
        // Register the Failure converter for detailed exception information
        converters.AddFailureConverter();
        
        var formatter = new YamlFormatter(new YamlFormatterOptions 
        { 
            Settings = new YamlSerializerOptions { Converters = converters },
            SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace | FaultSensitivityDetails.FailureWithData
        });
        
        try
        {
            throw new InvalidOperationException("An error occurred during processing.");
        }
        catch (Exception ex)
        {
            var failure = new Failure(ex, FaultSensitivityDetails.FailureWithStackTrace | FaultSensitivityDetails.FailureWithData);
            var yaml = formatter.Serialize(failure, typeof(Failure));
            using (var reader = new StreamReader(yaml))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }
    }
}
```
