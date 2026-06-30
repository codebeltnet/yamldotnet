---
uid: Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatterOptionsExtensions
example:
- *content
---

The following example demonstrates how to use the `SetPropertyName` extension method to apply naming convention transformations when building YAML output.

```csharp
using System;
using System.IO;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        // Create formatter options with camelCase naming convention
        var options = new YamlFormatterOptions
        {
            Settings = new YamlSerializerOptions
            {
                NamingConvention = CamelCaseNamingConvention.Instance
            }
        };
        
        // Manually create a YAML converter that uses the naming convention
        var converter = YamlConverterFactory.Create<PersonProfile>(
            writer: (emitter, person, formatter) =>
            {
                emitter.WriteStartObject();
                
                // Apply naming convention to property names
                emitter.WritePropertyName(options.SetPropertyName("FirstName"));
                emitter.WriteValue(person.FirstName);
                
                emitter.WritePropertyName(options.SetPropertyName("LastName"));
                emitter.WriteValue(person.LastName);
                
                emitter.WritePropertyName(options.SetPropertyName("EmailAddress"));
                emitter.WriteValue(person.EmailAddress);
                
                emitter.WriteEndObject();
            }
        );
        
        // Add converter to options
        options.Settings.Converters.Add(converter);
        
        // Use formatter to serialize
        var formatter = new Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatter(options);
        var person = new PersonProfile 
        { 
            FirstName = "Jane", 
            LastName = "Smith", 
            EmailAddress = "jane@example.com" 
        };
        
        var yaml = formatter.Serialize(person, typeof(PersonProfile));
        using (var reader = new StreamReader(yaml))
        {
            Console.WriteLine(reader.ReadToEnd());
        }
        // Output will use camelCase:
        // firstName: Jane
        // lastName: Smith
        // emailAddress: jane@example.com
    }
}

public class PersonProfile
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string EmailAddress { get; set; }
}
```
