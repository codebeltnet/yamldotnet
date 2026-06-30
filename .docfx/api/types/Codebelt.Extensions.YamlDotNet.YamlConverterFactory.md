---
uid: Codebelt.Extensions.YamlDotNet.YamlConverterFactory
example:
- *content
---

This example demonstrates how to create a custom YAML converter using `YamlConverterFactory` for a specific type and integrate it with `YamlFormatter`. It shows the workflow: defining a custom writer delegate that controls how a Person object is serialized (object start/end, properties), instantiating the factory to create the converter, adding the converter to the formatter's settings, and then serializing a Person instance to observe the custom YAML output. The observable outcome is that the Person object is serialized according to the custom writer logic rather than default YAML conventions.

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Core;

namespace ExampleNamespace;

class Person
{
    public required string Name { get; set; }
    public required int Age { get; set; }
}

class Program
{
    static void Main()
    {
        // Create a custom converter for the Person type
        var personConverter = YamlConverterFactory.Create<Person>(
            writer: (emitter, person, formatter) =>
            {
                emitter.WriteStartObject();
                emitter.WriteString("name", person.Name);
                emitter.WriteString("age", person.Age.ToString());
                emitter.WriteEndObject();
            }
        );
        
        // Add the custom converter to the formatter options
        var options = new YamlFormatterOptions { Settings = new YamlSerializerOptions() };
        options.Settings.Converters.Add(personConverter);
        
        // Use the formatter with the custom converter
        var formatter = new YamlFormatter(options);
        var person = new Person { Name = "John Doe", Age = 30 };
        var yaml = formatter.Serialize(person, typeof(Person));
        
        using (var reader = new StreamReader(yaml))
        {
            Console.WriteLine(reader.ReadToEnd());
        }
    }
}
```
