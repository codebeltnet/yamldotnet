---
uid: Codebelt.Extensions.YamlDotNet.EmitterExtensions
example:
- *content
---

This example demonstrates how to use the `EmitterExtensions` methods to manually construct YAML output with fine-grained control over structure and formatting. The workflow includes: initializing a StringWriter and Emitter instance, writing object properties using `WriteString` and `WritePropertyName`, creating nested arrays with `WriteStartArray`/`WriteEndArray`, and serializing complex objects with `WriteObject`. The observable outcome is a properly formatted YAML string that combines primitive values, arrays, and nested objects, demonstrating how each extension method contributes to building the complete YAML document structure.

```csharp
using System;
using System.IO;
using Codebelt.Extensions.YamlDotNet;
using Codebelt.Extensions.YamlDotNet.Formatters;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace ExampleNamespace;

public class AppConfig
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string[] Features { get; set; }
}

class Program
{
    static void Main()
    {
        var options = new YamlFormatterOptions();
        var stringWriter = new StringWriter();
        var emitter = new Emitter(stringWriter);
        
        // Use WriteStartObject and WriteString for simple key-value pairs
        emitter.WriteStartObject();
        emitter.WriteString("applicationName", "MyApp");
        emitter.WriteString("version", "1.0.0");
        
        // Use WriteStartArray and WriteEndArray for list values
        emitter.WritePropertyName("features");
        emitter.WriteStartArray();
        emitter.WriteValue("logging");
        emitter.WriteValue("metrics");
        emitter.WriteValue("debugging");
        emitter.WriteEndArray();
        
        // Use WriteObject to serialize a complex object
        emitter.WritePropertyName("configuration");
        var config = new AppConfig { Name = "Primary", Version = "2.0", Features = new[] { "auth", "caching" } };
        emitter.WriteObject(config, options);
        
        emitter.WriteEndObject();
        
        Console.WriteLine(stringWriter.ToString());
    }
}
```
