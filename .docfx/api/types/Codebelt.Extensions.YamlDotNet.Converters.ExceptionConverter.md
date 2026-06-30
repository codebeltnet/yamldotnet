---
uid: Codebelt.Extensions.YamlDotNet.Converters.ExceptionConverter
example:
- *content
---

This example demonstrates how to use the `ExceptionConverter` to serialize exceptions with two different detail configurations, showing how to control which exception information appears in the serialized YAML. It shows the workflow: creating an exception converter with specific settings (stack trace and data), adding it to the formatter options, then demonstrating two scenarios—one with full details and one with minimal details. The observable outcome is that the same exception appears differently in YAML format depending on the converter configuration.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Codebelt.Extensions.YamlDotNet.Converters;
using Codebelt.Extensions.YamlDotNet.Formatters;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        // Create an exception converter that includes stack traces and data
        var converter = new ExceptionConverter(includeStackTrace: true, includeData: true);
        
        // Create formatter options and add the converter
        var options = new YamlFormatterOptions();
        options.Settings.Converters.Add(converter);
        
        // Create a formatter with the custom converter
        var formatter = new YamlFormatter(options);
        
        // Demonstrate serialization with different levels of detail
        try
        {
            throw new InvalidOperationException("An error occurred during processing.", 
                new ArgumentException("Invalid argument provided"));
        }
        catch (Exception ex)
        {
            // Serialize with full details
            var yaml = formatter.Serialize(ex, typeof(Exception));
            var reader = new StreamReader(yaml);
            Console.WriteLine("Exception as YAML:");
            Console.WriteLine(reader.ReadToEnd());
        }
        
        // Converter without stack trace
        var minimalConverter = new ExceptionConverter(includeStackTrace: false, includeData: false);
        var minimalOptions = new YamlFormatterOptions();
        minimalOptions.Settings.Converters.Add(minimalConverter);
        
        try
        {
            throw new NullReferenceException("Value was null");
        }
        catch (Exception ex)
        {
            var formatter2 = new YamlFormatter(minimalOptions);
            var yaml = formatter2.Serialize(ex, typeof(Exception));
            var reader = new StreamReader(yaml);
            Console.WriteLine("\nMinimal exception details:");
            Console.WriteLine(reader.ReadToEnd());
        }
    }
}
```
