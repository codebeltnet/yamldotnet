---
uid: Codebelt.Extensions.YamlDotNet.ScalarOptions
example:
- *content
---

The following example demonstrates how to use `ScalarOptions` to control scalar value formatting (style, quoting) when writing YAML output.

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
        
        // Create scalar options for different formatting styles
        var literalStyle = new ScalarOptions
        {
            Style = ScalarStyle.Literal,
            IsKey = false
        };
        
        var quotedStyle = new ScalarOptions
        {
            Style = ScalarStyle.DoubleQuoted,
            IsPlainImplicit = false,
            IsKey = false
        };
        
        emitter.WriteStartObject();
        
        // Write value with literal style (preserves newlines and formatting)
        emitter.WritePropertyName("description");
        emitter.WriteValue(
            "This is a multi-line\ndescription that uses\nliteral style",
            o => o.Style = ScalarStyle.Literal
        );
        
        // Write value with quoted style
        emitter.WritePropertyName("code");
        emitter.WriteValue("special!chars@here", o => o.Style = ScalarStyle.DoubleQuoted);
        
        emitter.WriteEndObject();
        
        var yaml = stringWriter.ToString();
        Console.WriteLine(yaml);
    }
}
```
