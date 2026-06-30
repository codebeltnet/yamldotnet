---
uid: Codebelt.Extensions.YamlDotNet.NodeOptions
example:
- *content
---

The following example demonstrates how to use `NodeOptions` to configure YAML node properties like anchors and tags when writing low-level YAML output.

```csharp
using System;
using System.IO;
using Codebelt.Extensions.YamlDotNet;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        var stringWriter = new StringWriter();
        var emitter = new Emitter(stringWriter);
        
        // Create and configure node options with anchor and tag
        var nodeOptions = new NodeOptions
        {
            Anchor = new AnchorName("myanchor"),
            Tag = new TagName("!custom-tag")
        };
        
        // These options can be passed to emitter extensions when writing YAML
        // For example, when creating a custom converter
        
        // Write a simple configuration using basic NodeOptions
        emitter.WriteStartObject();
        emitter.WriteString("name", "Configuration");
        emitter.WriteString("value", "example");
        emitter.WriteEndObject();
        
        var yaml = stringWriter.ToString();
        Console.WriteLine(yaml);
    }
}
```
