---
uid: Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatterOptions
example:
- *content
---

The following example demonstrates how to configure `YamlFormatterOptions` to customize YAML serialization behavior, including converters, encoding, and fault sensitivity.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Codebelt.Extensions.YamlDotNet.Converters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Diagnostics;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        // Create options and configure defaults
        var options = new YamlFormatterOptions
        {
            // Set encoding for output
            Encoding = Encoding.UTF8,
            
            // Configure which exception details to include
            SensitivityDetails = FaultSensitivityDetails.FailureWithStackTrace | FaultSensitivityDetails.FailureWithData
        };
        
        // Add custom converters to the settings
        options.Settings.Converters.AddFailureConverter();
        
        // Use options to create a formatter
        var formatter = new YamlFormatter(options);
        
        // Now use the formatter for serialization
        try
        {
            throw new InvalidOperationException("Example error");
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
