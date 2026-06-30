---
uid: Codebelt.Extensions.YamlDotNet.Formatters.YamlFormatter
example:
- *content
---

This example demonstrates how to serialize and deserialize objects using `YamlFormatter` with custom encoding settings. It shows the complete workflow: creating an application configuration object, instantiating the formatter with UTF-8 encoding configuration, serializing the config to YAML, displaying the YAML text, then deserializing the YAML back to the original object type. The observable outcome is that the object can be round-tripped (serialized and deserialized) while maintaining its structure and data integrity.

```csharp
using System;
using System.IO;
using System.Text;
using Codebelt.Extensions.YamlDotNet.Formatters;

namespace ExampleNamespace;

public class AppConfiguration
{
    public string ApplicationName { get; set; }
    public string Version { get; set; }
    public int MaxConnections { get; set; }
}

class Program
{
    static void Main()
    {
        // Create and configure the formatter
        var formatter = new YamlFormatter(options =>
        {
            options.Settings.Encoding = Encoding.UTF8;
        });
        
        // Serialize an object to YAML
        var config = new AppConfiguration
        {
            ApplicationName = "MyApplication",
            Version = "1.0.0",
            MaxConnections = 100
        };
        
        var yamlStream = formatter.Serialize(config, typeof(AppConfiguration));
        var yamlText = new StreamReader(yamlStream).ReadToEnd();
        Console.WriteLine("Serialized YAML:");
        Console.WriteLine(yamlText);
        
        // Reset stream for deserialization
        yamlStream = new MemoryStream(Encoding.UTF8.GetBytes(yamlText));
        
        // Deserialize YAML back to object
        var deserializedConfig = (AppConfiguration)formatter.Deserialize(yamlStream, typeof(AppConfiguration));
        Console.WriteLine($"\nDeserialized config: {deserializedConfig.ApplicationName} v{deserializedConfig.Version}");
    }
}
```
