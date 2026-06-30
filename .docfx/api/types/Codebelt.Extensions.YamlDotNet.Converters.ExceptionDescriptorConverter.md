---
uid: Codebelt.Extensions.YamlDotNet.Converters.ExceptionDescriptorConverter
example:
- *content
---

This example demonstrates how to serialize exception objects using the `ExceptionDescriptorConverter` with customizable detail levels. It shows the complete workflow: creating an `ExceptionDescriptor` from a caught exception, instantiating the converter with fine-grained sensitivity settings (stack trace, data, and evidence), wrapping it in a `YamlFormatter`, and then serializing to observe the full exception information in YAML format. The example illustrates how to control what exception details appear in the output by configuring `FaultSensitivityDetails` flags on the converter.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Codebelt.Extensions.YamlDotNet.Converters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Diagnostics;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        // Create an exception and descriptor
        try
        {
            throw new InvalidOperationException("Operation failed.", 
                new ArgumentException("Invalid input"));
        }
        catch (Exception ex)
        {
            var descriptor = new ExceptionDescriptor(ex, "E001", "An operation error occurred.");
            
            // Create converter with custom sensitivity settings
            var converter = new ExceptionDescriptorConverter(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.StackTrace | 
                                            FaultSensitivityDetails.Data | 
                                            FaultSensitivityDetails.Evidence;
            });
            
            // Create formatter with the converter
            var formatterOptions = new YamlFormatterOptions();
            formatterOptions.Settings.Converters.Add(converter);
            
            var formatter = new YamlFormatter(formatterOptions);
            
            // Serialize the descriptor
            var yaml = formatter.Serialize(descriptor, typeof(ExceptionDescriptor));
            using (var reader = new StreamReader(yaml))
            {
                Console.WriteLine("Exception Descriptor as YAML:");
                Console.WriteLine(reader.ReadToEnd());
            }
        }
    }
}
```
