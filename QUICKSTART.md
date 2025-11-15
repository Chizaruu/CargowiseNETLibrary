# Quick Start Guide

Get up and running with CargoWiseNetLibrary in 5 minutes!

## ⚠️ Important Notice

**YOU MUST BE AN AUTHORIZED CARGOWISE USER** or Affiliated Partner to use this library. See [LEGAL.md](LEGAL.md) for details.

## Installation

### Option 1: NuGet Package Manager

```powershell
Install-Package CargoWiseNetLibrary
```

### Option 2: .NET CLI

```bash
dotnet add package CargoWiseNetLibrary
```

### Option 3: PackageReference

```xml
<PackageReference Include="CargoWiseNetLibrary" Version="1.0.0" />
```

## Basic Usage

### 1. Working with Universal Shipments

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using CargoWiseNetLibrary.Validation;

// Create a shipment
var shipment = new UniversalShipmentData
{
    Shipment = new Shipment
    {
        DataContext = new DataContext
        {
            EnterpriseID = "YOUR_ENT_ID",
            ServerID = "YOUR_SERVER",
            Company = new Company
            {
                Code = "TEST_COMPANY"
            }
        },
        OrderNumber = "ORDER-2025-001",
        // Add other shipment properties as needed
    }
};
```

### 2. XML Serialization

```csharp
using CargoWiseNetLibrary.Serialization;

// Serialize to XML string
var options = new XmlSerializerOptions
{
    Indent = true,
    OmitXmlDeclaration = false
};

var xml = XmlSerializer<UniversalShipmentData>.Serialize(shipment, options);
Console.WriteLine(xml);

// Save to file
XmlSerializer<UniversalShipmentData>.SerializeToFile(shipment, "shipment.xml", options);

// Load from file
var loadedShipment = XmlSerializer<UniversalShipmentData>.DeserializeFromFile("shipment.xml");

// Safe deserialization with validation
if (XmlSerializer<UniversalShipmentData>.TryDeserialize(xml, out var validShipment))
{
    Console.WriteLine("Successfully deserialized!");
    // Process validShipment
}
else
{
    Console.WriteLine("Failed to deserialize XML");
}

// Check if XML is valid before deserializing
if (XmlSerializer<UniversalShipmentData>.IsValid(xml))
{
    var data = XmlSerializer<UniversalShipmentData>.Deserialize(xml);
}
```

### 3. Working with UniversalInterchange

UniversalInterchange is the envelope format used by CargoWise for EDI messages. The library provides helper utilities to work with this format easily.

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Deserialize UniversalInterchange and extract shipment data
var xml = File.ReadAllText("interchange.xml");
var shipmentData = UniversalInterchangeHelper.DeserializeAndExtract<UniversalShipmentData>(xml);

if (shipmentData != null)
{
    Console.WriteLine("Successfully extracted shipment data!");
}

// Create UniversalInterchange wrapper around your data
var shipment = new UniversalShipmentData { /* ... */ };
var interchange = UniversalInterchangeHelper.CreateInterchange(shipment);

// Serialize with UniversalInterchange envelope
var wrappedXml = UniversalInterchangeHelper.SerializeWithInterchange(shipment);
File.WriteAllText("output.xml", wrappedXml);

// Safe extraction with error handling
if (UniversalInterchangeHelper.TryDeserializeAndExtract<UniversalShipmentData>(xml, out var data))
{
    // Successfully extracted
    ProcessShipment(data);
}
```

### 4. UniversalInterchange Extension Methods

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Load interchange
var interchange = XmlSerializer<UniversalInterchange>.Deserialize(xml);

// Check if it has data
if (interchange.HasData())
{
    // Get the data type
    var dataType = interchange.GetDataType();
    var typeName = interchange.GetDataTypeName();
    var elementName = interchange.GetElementName();

    Console.WriteLine($"Contains: {typeName}");
    Console.WriteLine($"Element: {elementName}");

    // Check for specific data type
    if (interchange.ContainsDataType<UniversalShipmentData>())
    {
        var shipmentData = interchange.ExtractData<UniversalShipmentData>();
        ProcessShipment(shipmentData);
    }
    else if (interchange.ContainsDataType<UniversalEventData>())
    {
        var eventData = interchange.ExtractData<UniversalEventData>();
        ProcessEvent(eventData);
    }

    // Safe extraction
    if (interchange.TryExtractData<UniversalShipmentData>(out var data))
    {
        Console.WriteLine("Successfully extracted shipment data");
    }
}
```

### 5. JSON Serialization

```csharp
using CargoWiseNetLibrary.Serialization;
using System.Text.Json;

// Convert to JSON
var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNameCaseInsensitive = true
};

var json = JsonSerializer<UniversalShipmentData>.Serialize(shipment, jsonOptions);
Console.WriteLine(json);

// Save to file
JsonSerializer<UniversalShipmentData>.SerializeToFile(shipment, "shipment.json", jsonOptions);

// Async file operations
await JsonSerializer<UniversalShipmentData>.SerializeToFileAsync(
    shipment,
    "shipment.json",
    jsonOptions);

var loaded = await JsonSerializer<UniversalShipmentData>.DeserializeFromFileAsync(
    "shipment.json",
    jsonOptions);

// Create a deep copy
var copy = JsonSerializer<UniversalShipmentData>.Clone(shipment, jsonOptions);

// Safe deserialization
if (JsonSerializer<UniversalShipmentData>.TryDeserialize(json, out var result, jsonOptions))
{
    Console.WriteLine("Successfully deserialized JSON");
}

// Validate JSON
if (JsonSerializer<UniversalShipmentData>.IsValid(json, jsonOptions))
{
    var data = JsonSerializer<UniversalShipmentData>.Deserialize(json, jsonOptions);
}
```

### 6. Validation

```csharp
using CargoWiseNetLibrary.Validation;

// Validate a single shipment
var validator = new ModelValidator<UniversalShipmentData>();
var result = validator.Validate(shipment);

if (result.IsValid)
{
    Console.WriteLine("✓ Shipment is valid");
}
else
{
    Console.WriteLine("✗ Validation errors:");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"  - {error.PropertyName}: {error.ErrorMessage}");
    }
}

// Safe validation with null check
if (validator.TryValidate(shipment, out var validationResult))
{
    if (validationResult.IsValid)
    {
        ProcessShipment(shipment);
    }
}

// Validate multiple shipments
var shipments = new[] { shipment1, shipment2, shipment3 };

// Quick check - returns true only if ALL are valid
bool allValid = validator.ValidateAll(shipments);

// Get detailed results for each
var results = validator.ValidateMany(shipments);
foreach (var kvp in results)
{
    Console.WriteLine($"Shipment: {kvp.Key.GetHashCode()}");
    Console.WriteLine($"Valid: {kvp.Value.IsValid}");
}

// Filter valid and invalid models
var validShipments = validator.GetValidModels(shipments);
var invalidShipments = validator.GetInvalidModels(shipments);

Console.WriteLine($"Valid: {validShipments.Count()}, Invalid: {invalidShipments.Count()}");

// Get all errors from a collection
var allErrors = validator.GetAllErrors(shipments);
foreach (var error in allErrors)
{
    Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
}

// Async validation
var asyncResult = await validator.ValidateAsync(shipment);
var asyncResults = await validator.ValidateManyAsync(shipments);
```

### 7. Working with Native Models

```csharp
using CargoWiseNetLibrary.Models.Native;
using CargoWiseNetLibrary.Serialization;

// Native models work the same way as Universal models
var nativeData = new Native { /* ... */ };

// Serialize
var xml = XmlSerializer<Native>.Serialize(nativeData);

// Deserialize
var loaded = XmlSerializer<Native>.Deserialize(xml);

// Validate
var validator = new ModelValidator<Native>();
var result = validator.Validate(nativeData);
```

## Common Patterns

### Pattern 1: Batch Import from XML Files

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using CargoWiseNetLibrary.Validation;

var files = Directory.GetFiles("imports", "*.xml");
var validator = new ModelValidator<UniversalShipmentData>();
var successfulImports = new List<UniversalShipmentData>();
var failedImports = new List<(string File, string Reason)>();

foreach (var file in files)
{
    try
    {
        // Try to deserialize
        if (!XmlSerializer<UniversalShipmentData>.TryDeserialize(
            File.ReadAllText(file), out var shipment) || shipment == null)
        {
            failedImports.Add((file, "Deserialization failed"));
            continue;
        }

        // Validate
        var result = validator.Validate(shipment);
        if (result.IsValid)
        {
            successfulImports.Add(shipment);
        }
        else
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            failedImports.Add((file, $"Validation failed: {errors}"));
        }
    }
    catch (Exception ex)
    {
        failedImports.Add((file, ex.Message));
    }
}

Console.WriteLine($"Successfully imported: {successfulImports.Count}");
Console.WriteLine($"Failed: {failedImports.Count}");

foreach (var (file, reason) in failedImports)
{
    Console.WriteLine($"  {file}: {reason}");
}
```

### Pattern 2: Convert XML to JSON

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Read XML file
var xml = File.ReadAllText("shipment.xml");
var shipment = XmlSerializer<UniversalShipmentData>.Deserialize(xml);

if (shipment != null)
{
    // Convert to JSON with pretty printing
    var json = JsonSerializer<UniversalShipmentData>.Serialize(shipment);
    File.WriteAllText("shipment.json", json);

    Console.WriteLine("Converted XML to JSON successfully!");
}
```

### Pattern 3: Process UniversalInterchange Files

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

var xml = File.ReadAllText("interchange.xml");
var interchange = XmlSerializer<UniversalInterchange>.Deserialize(xml);

if (interchange?.HasData() == true)
{
    var dataType = interchange.GetDataType();

    switch (dataType?.Name)
    {
        case nameof(UniversalShipmentData):
            var shipment = interchange.ExtractData<UniversalShipmentData>();
            ProcessShipment(shipment);
            break;

        case nameof(UniversalEventData):
            var eventData = interchange.ExtractData<UniversalEventData>();
            ProcessEvent(eventData);
            break;

        case nameof(UniversalTransactionData):
            var transaction = interchange.ExtractData<UniversalTransactionData>();
            ProcessTransaction(transaction);
            break;

        default:
            Console.WriteLine($"Unknown data type: {dataType?.Name}");
            break;
    }
}
```

### Pattern 4: Async File Processing with Validation

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using CargoWiseNetLibrary.Validation;

async Task ProcessShipmentsAsync(IEnumerable<string> files, CancellationToken ct = default)
{
    var validator = new ModelValidator<UniversalShipmentData>();

    foreach (var file in files)
    {
        try
        {
            // Load asynchronously
            var shipment = await XmlSerializer<UniversalShipmentData>
                .DeserializeFromFileAsync(file, ct);

            if (shipment == null)
            {
                Console.WriteLine($"Failed to load: {file}");
                continue;
            }

            // Validate asynchronously
            var result = await validator.ValidateAsync(shipment, ct);

            if (result.IsValid)
            {
                await ProcessValidShipmentAsync(shipment, ct);
            }
            else
            {
                Console.WriteLine($"Validation failed for {file}:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"  {error.PropertyName}: {error.ErrorMessage}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {file}: {ex.Message}");
        }
    }
}

async Task ProcessValidShipmentAsync(UniversalShipmentData shipment, CancellationToken ct)
{
    // Your business logic here
    await Task.CompletedTask;
}
```

### Pattern 5: Create and Send UniversalInterchange

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Create your data
var shipment = new UniversalShipmentData
{
    Shipment = new Shipment
    {
        DataContext = new DataContext
        {
            EnterpriseID = "YOUR_ENT_ID",
            ServerID = "YOUR_SERVER",
            Company = new Company
            {
                Code = "TEST_COMPANY"
            }
        },
        OrderNumber = "ORDER-2025-001"
    }
};

// Wrap in UniversalInterchange
var interchange = UniversalInterchangeHelper.CreateInterchange(shipment);

// Serialize with proper formatting
var options = new XmlSerializerOptions
{
    Indent = true,
    OmitXmlDeclaration = false
};

var xml = XmlSerializer<UniversalInterchange>.Serialize(interchange, options);

// Send to CargoWise API or save to file
await File.WriteAllTextAsync("output.xml", xml);

// Or use the helper method directly
var wrappedXml = UniversalInterchangeHelper.SerializeWithInterchange(shipment, options);
```

### Pattern 6: Deep Cloning Objects

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Original shipment
var originalShipment = new UniversalShipmentData { /* ... */ };

// Create a deep copy using XML serialization (preserves all XML attributes)
var xmlClone = XmlSerializer<UniversalShipmentData>.Clone(originalShipment);

// Or create a deep copy using JSON serialization (might be faster)
var jsonClone = JsonSerializer<UniversalShipmentData>.Clone(originalShipment);

// Modify the clone without affecting the original
xmlClone.Shipment.OrderNumber = "NEW-ORDER-123";

Console.WriteLine($"Original: {originalShipment.Shipment.OrderNumber}");
Console.WriteLine($"Clone: {xmlClone.Shipment.OrderNumber}");
```

### Pattern 7: Error Handling and Logging

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using CargoWiseNetLibrary.Validation;

async Task<bool> ProcessShipmentFileAsync(string filePath)
{
    try
    {
        // Check file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"File not found: {filePath}");
            return false;
        }

        // Load file
        var xml = await File.ReadAllTextAsync(filePath);

        // Validate XML format
        if (!XmlSerializer<UniversalShipmentData>.IsValid(xml))
        {
            Console.WriteLine($"Invalid XML format: {filePath}");
            return false;
        }

        // Deserialize
        var shipment = XmlSerializer<UniversalShipmentData>.Deserialize(xml);
        if (shipment == null)
        {
            Console.WriteLine($"Deserialization returned null: {filePath}");
            return false;
        }

        // Validate model
        var validator = new ModelValidator<UniversalShipmentData>();
        var result = validator.Validate(shipment);

        if (!result.IsValid)
        {
            Console.WriteLine($"Validation errors in {filePath}:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  [{error.PropertyName}] {error.ErrorMessage}");
            }
            return false;
        }

        // Process shipment
        await ProcessShipmentAsync(shipment);
        Console.WriteLine($"Successfully processed: {filePath}");
        return true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing {filePath}: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        return false;
    }
}

async Task ProcessShipmentAsync(UniversalShipmentData shipment)
{
    // Your business logic here
    await Task.CompletedTask;
}
```

## Tips and Best Practices

### ✅ Do's

1. **Always validate** before processing data - use `ModelValidator<T>` to ensure data integrity
2. **Use async methods** for file operations in web applications to avoid blocking
3. **Handle exceptions** appropriately with try-catch blocks
4. **Use TryDeserialize** for safer deserialization with error handling
5. **Leverage Clone methods** when you need a deep copy of an object
6. **Use UniversalInterchangeHelper** when working with CargoWise EDI envelopes
7. **Check IsValid** before deserializing untrusted data
8. **Use proper XML options** to match CargoWise expected format
9. **Filter collections** with `GetValidModels()` and `GetInvalidModels()`
10. **Log validation errors** for troubleshooting and debugging

### ❌ Don'ts

1. **Don't ignore validation results** - they contain important error information
2. **Don't deserialize untrusted XML/JSON** without validation
3. **Don't use synchronous file operations** in async contexts (use `*Async` methods)
4. **Don't hardcode namespaces** - use `XmlSerializerOptions` for flexibility
5. **Don't assume deserialization succeeded** - always check for null results
6. **Don't process large batches** without error handling for individual items
7. **Don't forget to wrap data** in UniversalInterchange when sending to CargoWise
8. **Don't use this library** without proper CargoWise authorization
9. **Don't modify generated model files** - they will be overwritten on regeneration
10. **Don't skip async/await** when using async methods

## Performance Tips

1. **Batch validation**: Use `ValidateAll()` for quick checks, `ValidateMany()` when you need individual results
2. **Async operations**: Always use async methods for I/O operations in production
3. **JSON vs XML**: JSON serialization is generally faster; XML preserves all attributes
4. **Parallel processing**: Process multiple files in parallel with `Task.WhenAll()`
5. **Memory management**: For very large models, process in chunks to avoid memory issues

## Common Issues and Solutions

### Issue: Deserialization returns null

**Solution**:

- Verify XML/JSON format matches the expected schema
- Use `TryDeserialize` to catch errors
- Check namespace declarations in XML
- Ensure all required fields are present

### Issue: Validation fails unexpectedly

**Solution**:

- Review `ValidationResult.Errors` for specific issues
- Check for `[Required]` attributes on properties
- Verify string length constraints with `[MaxLength]`
- Ensure all nested objects are properly initialized

### Issue: UniversalInterchange extraction fails

**Solution**:

- Verify XML contains proper UniversalInterchange envelope
- Use `HasData()` to check if body contains data
- Use `GetDataType()` to see what type is actually present
- Try `TryExtractData<T>()` for safer extraction

## Next Steps

- 📖 Read the full [README.md](README.md) for comprehensive documentation
- 🔧 Check [SCHEMA_GENERATION.md](SCHEMA_GENERATION.md) for model regeneration
- 📄 Review [LEGAL.md](LEGAL.md) for authorization requirements
- 🤝 Read [CONTRIBUTING.md](CONTRIBUTING.md) to contribute
- 🐛 Report issues on [GitHub Issues](https://github.com/Chizaruu/CargoWiseNetLibrary/issues)
- ⭐ Star the project if you find it useful!

## Need Help?

- 📖 [Full Documentation](README.md)
- 📋 [API Reference](README.md#api-reference)
- 🐛 [Report Issues](https://github.com/Chizaruu/CargoWiseNetLibrary/issues)
- 💬 [Discussions](https://github.com/Chizaruu/CargoWiseNetLibrary/discussions)
- 📧 Contact: contact@koalahollow.com

## Sample Projects

Check out the `examples/` directory in the repository for complete sample projects demonstrating:

- Web API integration with CargoWise
- Batch file processing
- Real-time event handling
- Data transformation pipelines
- Error handling patterns

---

**Ready to integrate with CargoWise?** Start building today! 🚀

**Remember**: You must be an authorized CargoWise user to use this library. See [LEGAL.md](LEGAL.md) for complete legal information.
