---
uid: Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml.YamlSerializationMvcOptionsSetup
example:
- *content
---

The following example shows how to instantiate and use the `YamlSerializationMvcOptionsSetup` class to manually configure YAML input and output formatters on `MvcOptions`.

```csharp
using System;
using System.Linq;
using Codebelt.Extensions.AspNetCore.Mvc.Formatters.Text.Yaml;
using Codebelt.Extensions.YamlDotNet.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExampleNamespace;

class Program
{
    static void Main()
    {
        // Create default YamlFormatterOptions
        var formatterOptions = new OptionsWrapper<YamlFormatterOptions>(new YamlFormatterOptions());
        
        // Instantiate YamlSerializationMvcOptionsSetup with the options
        var setup = new YamlSerializationMvcOptionsSetup(formatterOptions);
        
        // Create a MvcOptions instance to be configured
        var mvcOptions = new MvcOptions();
        
        // Verify the initial state - no YAML formatters yet
        int initialInputCount = mvcOptions.InputFormatters.Count;
        int initialOutputCount = mvcOptions.OutputFormatters.Count;
        Console.WriteLine($"Before configuration:");
        Console.WriteLine($"  Input formatters: {initialInputCount}");
        Console.WriteLine($"  Output formatters: {initialOutputCount}");
        
        // Apply the configuration by calling the setup action
        setup.Configure(mvcOptions);
        
        // After configuration, YAML formatters are added to MvcOptions
        int afterInputCount = mvcOptions.InputFormatters.Count;
        int afterOutputCount = mvcOptions.OutputFormatters.Count;
        Console.WriteLine($"After YamlSerializationMvcOptionsSetup configuration:");
        Console.WriteLine($"  Input formatters: {afterInputCount}");
        Console.WriteLine($"  Output formatters: {afterOutputCount}");
        
        var hasYamlInput = mvcOptions.InputFormatters.OfType<YamlSerializationInputFormatter>().Any();
        var hasYamlOutput = mvcOptions.OutputFormatters.OfType<YamlSerializationOutputFormatter>().Any();
        Console.WriteLine($"  YAML input formatter added: {hasYamlInput}");
        Console.WriteLine($"  YAML output formatter added: {hasYamlOutput}");
    }
}
```
