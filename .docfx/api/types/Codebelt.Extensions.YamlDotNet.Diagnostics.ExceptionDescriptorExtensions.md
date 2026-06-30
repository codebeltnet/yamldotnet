---
uid: Codebelt.Extensions.YamlDotNet.Diagnostics.ExceptionDescriptorExtensions
example:
- *content
---

This example demonstrates how to use the `ToYaml` extension method to convert an `ExceptionDescriptor` to a YAML string representation with two different sensitivity configurations. It shows the workflow: catching an exception with an inner exception, wrapping it in an `ExceptionDescriptor`, and then calling `ToYaml()` twice—first with default settings to observe minimal details, then with expanded settings to show stack traces, data, and evidence. The observable outcome shows how the same exception appears differently in YAML format depending on the configured `FaultSensitivityDetails` flags.

```csharp
using System;
using Codebelt.Extensions.YamlDotNet.Diagnostics;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Cuemon.Diagnostics;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        try
        {
            // Simulate an error
            throw new InvalidOperationException("Database connection failed.", 
                new TimeoutException("Connection timeout"));
        }
        catch (Exception ex)
        {
            // Create an exception descriptor
            var descriptor = new ExceptionDescriptor(ex, "E500", "Database Error");
            
            // Convert to YAML with default options
            var yamlDefault = descriptor.ToYaml();
            Console.WriteLine("Default YAML output:");
            Console.WriteLine(yamlDefault);
            Console.WriteLine();
            
            // Convert with custom sensitivity settings
            var yamlWithDetails = descriptor.ToYaml(options =>
            {
                options.SensitivityDetails = FaultSensitivityDetails.StackTrace | 
                                            FaultSensitivityDetails.Data | 
                                            FaultSensitivityDetails.Evidence;
            });
            Console.WriteLine("YAML with full details:");
            Console.WriteLine(yamlWithDetails);
        }
    }
}
```
