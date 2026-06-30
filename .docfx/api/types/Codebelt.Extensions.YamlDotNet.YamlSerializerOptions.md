---
uid: Codebelt.Extensions.YamlDotNet.YamlSerializerOptions
example:
- *content
---

The following example demonstrates how to configure `YamlSerializerOptions` to customize YAML serialization behavior for the `SerializerBuilder` and `DeserializerBuilder`.

```csharp
using System;
using System.IO;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Converters;
using Codebelt.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ExampleNamespace;

public class Person
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}

class Program
{
    static void Main()
    {
        // Create and configure serializer options
        var options = new YamlSerializerOptions
        {
            // Use camelCase for property names in YAML
            NamingConvention = CamelCaseNamingConvention.Instance,
            
            // Configure indentation
            WhiteSpaceIndentation = 2,
            
            // Do not use YAML aliases for duplicate objects
            UseAliases = false,
            
            // Ensure data roundtrips correctly
            EnsureRoundtrip = false
        };
        
        // Add custom converters
        options.Converters.AddFailureConverter();
        
        // Create a formatter using these options
        var yamlFormatter = new Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatter(new YamlFormatterOptions { Settings = options });
        
        var person = new Person { FirstName = "John", LastName = "Doe" };
        var yaml = yamlFormatter.Serialize(person, typeof(Person));
        
        using (var reader = new StreamReader(yaml))
        {
            Console.WriteLine(reader.ReadToEnd());
        }
    }
}
```
