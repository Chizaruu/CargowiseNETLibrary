# CargoWiseNetLibrary

[![NuGet](https://img.shields.io/nuget/v/CargoWiseNetLibrary.svg)](https://www.nuget.org/packages/CargoWiseNetLibrary/)
[![License: Apache 2.0](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Models Updated](https://img.shields.io/badge/Models_Updated-November_16_2025-brightgreen)](SCHEMA_GENERATION.md)

A comprehensive .NET 8 library for CargoWise EDI integration, including C# models generated from CargoWise XSD schemas with advanced serialization, validation, and helper utilities.

## ⚠️ IMPORTANT LEGAL NOTICE - YOU MUST BE AUTHORIZED

**USING THIS LIBRARY REQUIRES CARGOWISE AUTHORIZATION**

- CargoWise® and WiseTech Global® are registered trademarks of WiseTech Global Limited
- **This library includes C# models generated from CargoWise XSD schemas**
- The library maintainer is an authorized CargoWise user with schema access via EDI Messaging
- **⚠️ YOU MUST BE AN AUTHORIZED CARGOWISE USER OR AFFILIATED PARTNER TO USE THIS LIBRARY**
- Having these models does NOT grant you permission to use CargoWise systems
- You MUST comply with WiseTech Global's terms of use

**📄 Read the full legal disclaimer**: [LEGAL.md](LEGAL.md)

### For WiseTech Global Representatives

We respect your intellectual property rights. The maintainer is an authorized CargoWise user with access to schemas through the CargoWise application's EDI Messaging functionality. If you have any concerns about this repository, please contact us at contact@koalahollow.com, and we will respond within 48 business hours.

**This project is NOT affiliated with, endorsed by, or sponsored by WiseTech Global Limited.**

---

## Features

- ✅ **Universal Schema Models** - Complete CargoWise Universal EDI models in consolidated files
- ✅ **Native Schema Models** - Complete CargoWise Native EDI models in consolidated files
- ✅ **XML Serialization** - Full XML serialization/deserialization with proper attributes and options
- ✅ **JSON Support** - Modern JSON serialization using System.Text.Json
- ✅ **UniversalInterchange Helpers** - Specialized utilities for working with UniversalInterchange envelopes
- ✅ **Validation Framework** - Comprehensive validation using Data Annotations with multiple validation modes
- ✅ **Async Support** - Full async/await support for file operations
- ✅ **Clone Operations** - Deep cloning using serialization
- ✅ **TryDeserialize Patterns** - Safe deserialization with error handling
- ✅ **Batch Processing** - Validate and process multiple models efficiently
- ✅ **Strongly Typed** - Generated from XSD schemas for compile-time safety
- ✅ **Well Documented** - Comprehensive XML documentation for IntelliSense
- ✅ **.NET 8** - Built on the latest .NET platform

## Installation

### Via NuGet Package Manager

```powershell
Install-Package CargoWiseNetLibrary
```

### Via .NET CLI

```bash
dotnet add package CargoWiseNetLibrary
```

### Via PackageReference

```xml
<PackageReference Include="CargoWiseNetLibrary" Version="0.1.1" />
```

## Quick Start

### XML Serialization

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Deserialize XML to Universal models
var xmlContent = File.ReadAllText("shipment.xml");
var shipment = XmlSerializer<UniversalShipmentData>.Deserialize(xmlContent);

// Serialize models back to XML
var options = new XmlSerializerOptions
{
    Indent = true,
    OmitXmlDeclaration = false
};
var xml = XmlSerializer<UniversalShipmentData>.Serialize(shipment, options);
File.WriteAllText("output.xml", xml);

// Safe deserialization with error handling
if (XmlSerializer<UniversalShipmentData>.TryDeserialize(xmlContent, out var result))
{
    Console.WriteLine("Deserialization successful!");
}
```

### Working with UniversalInterchange

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Deserialize UniversalInterchange and extract specific data type
var xml = File.ReadAllText("interchange.xml");
var shipmentData = UniversalInterchangeHelper.DeserializeAndExtract<UniversalShipmentData>(xml);

// Create UniversalInterchange wrapper around data
var shipment = new UniversalShipmentData { /* ... */ };
var interchange = UniversalInterchangeHelper.CreateInterchange(shipment);

// Serialize with UniversalInterchange wrapper
var wrappedXml = UniversalInterchangeHelper.SerializeWithInterchange(shipment);

// Use extension methods
var interchange = XmlSerializer<UniversalInterchange>.Deserialize(xml);
if (interchange.HasData())
{
    var dataType = interchange.GetDataType();
    var typeName = interchange.GetDataTypeName();

    if (interchange.ContainsDataType<UniversalShipmentData>())
    {
        var data = interchange.ExtractData<UniversalShipmentData>();
    }
}
```

### Model Generation Status

- **Last Updated**: November 16, 2025
- **Schema Version**: CargoWise Universal and Native EDI Schemas
- **Generation Tool**: [Chizaruu.NetTools.SchemaGenerator](https://github.com/Chizaruu/Chizaruu.NetTools) (built on XmlSchemaClassGenerator v2.1.1183.0)
- **Models Included**:
  - Universal Schema Models (UniversalShipmentData, UniversalEventData, UniversalInterchange, etc.)
  - Native Schema Models (complete Native API support)
- **File Organization**: Consolidated into `Universal.cs` (57,406 lines) and `Native.cs` (192,814 lines)

### JSON Serialization

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using System.Text.Json;

// Convert Universal models to JSON
var shipment = new UniversalShipmentData { /* ... */ };
var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNameCaseInsensitive = true
};
var json = JsonSerializer<UniversalShipmentData>.Serialize(shipment, jsonOptions);

// Deserialize JSON back to models
var shipmentFromJson = JsonSerializer<UniversalShipmentData>.Deserialize(json, jsonOptions);

// Deep clone using JSON serialization
var clonedShipment = JsonSerializer<UniversalShipmentData>.Clone(shipment);

// Validate JSON before deserializing
if (JsonSerializer<UniversalShipmentData>.IsValid(json))
{
    var data = JsonSerializer<UniversalShipmentData>.Deserialize(json);
}

// Async file operations
await JsonSerializer<UniversalShipmentData>.SerializeToFileAsync(shipment, "shipment.json");
var loadedShipment = await JsonSerializer<UniversalShipmentData>.DeserializeFromFileAsync("shipment.json");
```

### Validation

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Validation;

var shipment = new UniversalShipmentData { /* ... */ };

// Single model validation
var validator = new ModelValidator<UniversalShipmentData>();
var result = validator.Validate(shipment);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"Validation Error: {error.PropertyName} - {error.ErrorMessage}");
    }
}

// Safe validation with null check
if (validator.TryValidate(shipment, out var validationResult))
{
    // Model was not null and validation was performed
    if (validationResult.IsValid)
    {
        Console.WriteLine("Model is valid!");
    }
}

// Validate multiple models
var shipments = new List<UniversalShipmentData> { /* ... */ };
var results = validator.ValidateMany(shipments);

// Check if all models are valid
bool allValid = validator.ValidateAll(shipments);

// Get only valid or invalid models
var validShipments = validator.GetValidModels(shipments);
var invalidShipments = validator.GetInvalidModels(shipments);

// Get all errors from a collection
var allErrors = validator.GetAllErrors(shipments);

// Get count of invalid models
int invalidCount = validator.GetInvalidCount(shipments);

// Async validation
var asyncResult = await validator.ValidateAsync(shipment);
var asyncResults = await validator.ValidateManyAsync(shipments);
```

## Project Structure

```
CargoWiseNetLibrary/
├── CargoWiseNetLibrary/                    # Main library project
│   ├── Models/
│   │   ├── Universal/                      # Universal schema models
│   │   │   └── Universal.cs                # All Universal models (57,406 lines)
│   │   └── Native/                         # Native schema models
│   │       └── Native.cs                   # All Native models (192,814 lines)
│   ├── Serialization/                      # Serialization helpers
│   │   ├── XmlSerializer.cs                # XML serialization with options
│   │   ├── JsonSerializer.cs               # JSON serialization with options
│   │   └── UniversalInterchangeHelper.cs   # UniversalInterchange utilities
│   └── Validation/                         # Validation utilities
│       ├── ModelValidator.cs               # Comprehensive validation framework
│       └── ValidationResult.cs             # Validation result models
├── CargoWiseNetLibrary.Tests/              # Unit tests
├── LICENSE
└── README.md
```

## Model Generation

All C# models in this library are generated from CargoWise XSD schemas using **[XmlSchemaClassGenerator](https://github.com/mganss/XmlSchemaClassGenerator)**, an industry-standard XSD-to-C# code generator.

**For detailed information on:**

- How models are generated
- How to regenerate models when schemas update
- Command-line usage and options
- Handling duplicates and troubleshooting

**See**: [SCHEMA_GENERATION.md](SCHEMA_GENERATION.md)

### Quick Regeneration

The library uses XmlSchemaClassGenerator for reliable, repeatable code generation:

```bash
# Install XmlSchemaClassGenerator
dotnet tool install --global XmlSchemaClassGenerator

# Generate Universal models (single consolidated file)
dotnet xscgen \
  -n "http://www.cargowise.com/Schemas/Universal/2011/11=CargoWiseNetLibrary.Models.Universal" \
  -o ./CargoWiseNetLibrary/Models/Universal \
  ./schemas/universal/*.xsd

# Generate Native models (single consolidated file)
dotnet xscgen \
  -n "http://www.cargowise.com/Schemas/Native/2011/11=CargoWiseNetLibrary.Models.Native" \
  -o ./CargoWiseNetLibrary/Models/Native \
  ./schemas/native/*.xsd
```

**Note**: Legal headers are manually added to generated files to ensure users understand authorization requirements.

## Universal and Native Models

The library includes comprehensive support for both CargoWise schemas:

### Universal Schema Models (auto-generated)

- **UniversalShipmentData** - Shipment and consignment data
- **UniversalEventData** - Event and milestone tracking
- **UniversalInterchange** - EDI interchange envelope
- **UniversalResponseData** - API response models
- **UniversalScheduleData** - Schedule information
- **UniversalTransactionData** - Transaction records
- **UniversalTransactionBatchData** - Batch transaction processing
- And many more...

### Native Schema Models (auto-generated)

- Complete CargoWise Native API model support
- All native data structures for advanced integrations
- Full support for CargoWise native workflows

All models include:

- XML serialization attributes (`[XmlElement]`, `[XmlAttribute]`, etc.)
- Data annotation validation attributes (`[Required]`, `[MaxLength]`, etc.)
- Nullable reference types support
- IntelliSense documentation
- Legal notice headers for compliance and authorization requirements

## Legal Headers in Generated Models

Every generated model file includes a legal header with important authorization information:

```csharp
///
/// ⚠️ IMPORTANT LEGAL NOTICE:
/// This file contains C# models generated from CargoWise XSD schemas.
/// The library maintainer is an authorized CargoWise user with schema access via EDI Messaging.
///
/// TO USE THIS LIBRARY:
/// - You MUST be an authorized CargoWise User or Affiliated Partner
/// - Having these models does NOT grant you permission to use CargoWise systems
/// - You MUST comply with all terms of your CargoWise user agreement
/// - XSD schemas are available to authorized users via CargoWise EDI Messaging
/// - See LEGAL.md for complete legal information
///
/// CargoWise® is a registered trademark of WiseTech Global Limited.
///
```

This ensures all users understand the authorization requirements when using the library.

## Advanced Usage

### Custom XML Options

```csharp
using CargoWiseNetLibrary.Serialization;

var options = new XmlSerializerOptions
{
    Indent = true,
    OmitXmlDeclaration = false,
    Namespace = "http://www.cargowise.com/Schemas/Universal/2011/11"
};

var xml = XmlSerializer<UniversalShipmentData>.Serialize(shipment, options);
```

### Async File Operations

```csharp
using CargoWiseNetLibrary.Serialization;

// XML async operations
await XmlSerializer<UniversalShipmentData>.SerializeToFileAsync(
    shipment,
    "shipment.xml",
    options,
    cancellationToken);

var loadedShipment = await XmlSerializer<UniversalShipmentData>
    .DeserializeFromFileAsync("shipment.xml", cancellationToken);

// JSON async operations
await JsonSerializer<UniversalShipmentData>.SerializeToFileAsync(
    shipment,
    "shipment.json",
    jsonOptions,
    cancellationToken);

var jsonShipment = await JsonSerializer<UniversalShipmentData>
    .DeserializeFromFileAsync("shipment.json", jsonOptions, cancellationToken);
```

### Deep Cloning

```csharp
using CargoWiseNetLibrary.Serialization;

// Clone using XML serialization (preserves XML attributes)
var xmlClone = XmlSerializer<UniversalShipmentData>.Clone(originalShipment);

// Clone using JSON serialization (may be faster for large objects)
var jsonClone = JsonSerializer<UniversalShipmentData>.Clone(originalShipment);
```

### Safe Deserialization Patterns

```csharp
using CargoWiseNetLibrary.Serialization;

// XML TryDeserialize
if (XmlSerializer<UniversalShipmentData>.TryDeserialize(xml, out var shipment))
{
    // Successfully deserialized
    ProcessShipment(shipment);
}

// JSON TryDeserialize
if (JsonSerializer<UniversalShipmentData>.TryDeserialize(json, out var jsonShipment, options))
{
    // Successfully deserialized
    ProcessShipment(jsonShipment);
}

// Validate before deserializing
bool isValidXml = XmlSerializer<UniversalShipmentData>.IsValid(xml);
bool isValidJson = JsonSerializer<UniversalShipmentData>.IsValid(json, options);
```

### Batch Processing with Validation

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using CargoWiseNetLibrary.Validation;

var files = Directory.GetFiles("imports", "*.xml");
var validator = new ModelValidator<UniversalShipmentData>();
var validShipments = new List<UniversalShipmentData>();
var errors = new List<string>();

foreach (var file in files)
{
    try
    {
        // Try to deserialize
        if (!XmlSerializer<UniversalShipmentData>.TryDeserialize(
            File.ReadAllText(file), out var data) || data == null)
        {
            errors.Add($"{file}: Failed to deserialize");
            continue;
        }

        // Validate
        var validationResult = validator.Validate(data);
        if (validationResult.IsValid)
        {
            validShipments.Add(data);
        }
        else
        {
            errors.Add($"{file}: Validation failed");
            foreach (var error in validationResult.Errors)
            {
                errors.Add($"  - {error.PropertyName}: {error.ErrorMessage}");
            }
        }
    }
    catch (Exception ex)
    {
        errors.Add($"{file}: {ex.Message}");
    }
}

Console.WriteLine($"Successfully processed {validShipments.Count} files");
Console.WriteLine($"Errors: {errors.Count}");

// Export valid shipments to JSON
foreach (var shipment in validShipments)
{
    var json = JsonSerializer<UniversalShipmentData>.Serialize(shipment);
    // Process JSON...
}
```

### Advanced UniversalInterchange Operations

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

// Load interchange and determine data type
var interchange = XmlSerializer<UniversalInterchange>.Deserialize(xml);

if (interchange.HasData())
{
    var dataType = interchange.GetDataType();
    var elementName = interchange.GetElementName();

    Console.WriteLine($"Contains: {interchange.GetDataTypeName()}");

    // Extract based on detected type
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

        // ... handle other types
    }
}

// Safe extraction with TryExtract
if (interchange.TryExtractData<UniversalShipmentData>(out var shipmentData))
{
    // Successfully extracted
    ProcessShipment(shipmentData);
}

// Check for specific data type
if (interchange.ContainsDataType<UniversalShipmentData>())
{
    var data = interchange.ExtractData<UniversalShipmentData>();
}
```

### Comprehensive Validation Workflow

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Validation;

var shipments = LoadShipmentsFromDatabase();
var validator = new ModelValidator<UniversalShipmentData>();

// Get validation results for all models
var results = validator.ValidateMany(shipments);

// Separate valid and invalid
var validShipments = validator.GetValidModels(shipments);
var invalidShipments = validator.GetInvalidModels(shipments);

Console.WriteLine($"Valid: {validShipments.Count()}");
Console.WriteLine($"Invalid: {invalidShipments.Count()}");

// Get all validation errors
var allErrors = validator.GetAllErrors(shipments);
foreach (var error in allErrors)
{
    Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
}

// Process only valid shipments
foreach (var validShipment in validShipments)
{
    await SendToCargoWise(validShipment);
}

// Log invalid shipments for review
foreach (var invalidShipment in invalidShipments)
{
    var result = validator.Validate(invalidShipment);
    LogValidationErrors(invalidShipment, result.Errors);
}

// Async validation for large datasets
var asyncResults = await validator.ValidateManyAsync(shipments, cancellationToken);
```

## API Reference

### XmlSerializer<T>

| Method                                                                          | Description                              |
| ------------------------------------------------------------------------------- | ---------------------------------------- |
| `Serialize(T obj, XmlSerializerOptions? options = null)`                        | Serializes object to XML string          |
| `Deserialize(string xml)`                                                       | Deserializes XML string to object        |
| `SerializeToFile(T obj, string filePath, XmlSerializerOptions? options = null)` | Serializes to XML file                   |
| `SerializeToFileAsync(...)`                                                     | Async version of SerializeToFile         |
| `DeserializeFromFile(string filePath)`                                          | Deserializes from XML file               |
| `DeserializeFromFileAsync(...)`                                                 | Async version of DeserializeFromFile     |
| `TryDeserialize(string xml, out T? result)`                                     | Safe deserialization with error handling |
| `IsValid(string xml)`                                                           | Validates XML can be deserialized        |
| `Clone(T obj, XmlSerializerOptions? options = null)`                            | Creates deep clone via serialization     |

### JsonSerializer<T>

| Method                                                                              | Description                              |
| ----------------------------------------------------------------------------------- | ---------------------------------------- |
| `Serialize(T obj, JsonSerializerOptions? options = null)`                           | Serializes object to JSON string         |
| `Deserialize(string json, JsonSerializerOptions? options = null)`                   | Deserializes JSON string to object       |
| `SerializeToFile(...)`                                                              | Serializes to JSON file                  |
| `SerializeToFileAsync(...)`                                                         | Async version of SerializeToFile         |
| `DeserializeFromFile(...)`                                                          | Deserializes from JSON file              |
| `DeserializeFromFileAsync(...)`                                                     | Async version of DeserializeFromFile     |
| `Clone(T obj, JsonSerializerOptions? options = null)`                               | Creates deep clone via serialization     |
| `TryDeserialize(string json, out T? result, JsonSerializerOptions? options = null)` | Safe deserialization with error handling |
| `IsValid(string json, JsonSerializerOptions? options = null)`                       | Validates JSON can be deserialized       |

### UniversalInterchangeHelper

| Method                                                                      | Description                                         |
| --------------------------------------------------------------------------- | --------------------------------------------------- |
| `DeserializeAndExtract<T>(string xml)`                                      | Deserializes UniversalInterchange and extracts data |
| `TryDeserializeAndExtract<T>(string xml, out T? data)`                      | Safe version with error handling                    |
| `CreateInterchange<T>(T data)`                                              | Creates UniversalInterchange wrapper                |
| `SerializeWithInterchange<T>(T data, XmlSerializerOptions? options = null)` | Serializes data wrapped in UniversalInterchange     |

### UniversalInterchange Extension Methods

| Method                           | Description                           |
| -------------------------------- | ------------------------------------- |
| `ExtractData<T>()`               | Extracts specific data type from body |
| `TryExtractData<T>(out T? data)` | Safe extraction with error handling   |
| `GetDataType()`                  | Gets Type of data in body             |
| `GetDataTypeName()`              | Gets name of data type                |
| `GetElementName()`               | Gets root element name                |
| `ContainsDataType<T>()`          | Checks if contains specific type      |
| `GetData()`                      | Gets untyped data object              |
| `HasData()`                      | Checks if body has any data           |

### ModelValidator<T>

| Method                                                   | Description                     |
| -------------------------------------------------------- | ------------------------------- |
| `Validate(T model)`                                      | Validates single model          |
| `ValidateAsync(T model, CancellationToken ct = default)` | Async single validation         |
| `ValidateMany(IEnumerable<T> models)`                    | Validates multiple models       |
| `ValidateManyAsync(...)`                                 | Async multiple validation       |
| `ValidateAll(IEnumerable<T> models)`                     | Returns true if all valid       |
| `ValidateAllAsync(...)`                                  | Async version of ValidateAll    |
| `TryValidate(T? model, out ValidationResult? result)`    | Safe validation with null check |
| `GetAllErrors(IEnumerable<T> models)`                    | Gets all validation errors      |
| `GetInvalidCount(IEnumerable<T> models)`                 | Counts invalid models           |
| `GetValidModels(IEnumerable<T> models)`                  | Filters to valid models only    |
| `GetInvalidModels(IEnumerable<T> models)`                | Filters to invalid models only  |

## Requirements

- .NET 8.0 or higher
- System.Text.Json (included in .NET 8)
- System.ComponentModel.Annotations (included in .NET 8)
- System.Xml.Serialization (included in .NET 8)

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

### Development Setup

1. Clone the repository

```bash
git clone https://github.com/Chizaruu/CargoWiseNetLibrary.git
cd CargoWiseNetLibrary
```

2. Build the solution

```bash
dotnet build
```

3. Run tests

```bash
dotnet test
```

## Performance Tips

- **XML vs JSON**: XML serialization preserves all XSD attributes; JSON is generally faster for large objects
- **Async Operations**: Use async methods for file I/O to avoid blocking
- **Validation**: Use `ValidateAll()` for quick checks, `ValidateMany()` when you need individual results
- **Batch Processing**: Process files in parallel when dealing with large datasets
- **Memory**: For very large models, consider streaming approaches or chunking

## Troubleshooting

### Common Issues

**Q: Deserialization returns null**

- Check XML/JSON format matches expected schema
- Use `TryDeserialize` to catch errors
- Verify namespace declarations in XML

**Q: Validation fails unexpectedly**

- Check for required fields with `[Required]` attribute
- Verify string length constraints with `[MaxLength]`
- Review validation errors in `ValidationResult.Errors`

**Q: UniversalInterchange extraction fails**

- Ensure XML contains proper UniversalInterchange envelope
- Use `HasData()` to check if body contains data
- Try `GetDataType()` to see what type is actually present

## License

This library's **code and infrastructure** is licensed under the Apache License 2.0 - see the [LICENSE](LICENSE) file for details.

**IMPORTANT**: CargoWise XSD schemas remain the property of WiseTech Global Limited. Users must be authorized CargoWise Users or Affiliated Partners to use this library.

See [LEGAL.md](LEGAL.md) for complete legal information and disclaimers.

## Acknowledgments

- WiseTech Global and CargoWise for their comprehensive EDI schemas
- The .NET community for excellent serialization and validation frameworks
- [Chizaruu.NetTools](https://github.com/Chizaruu/Chizaruu.NetTools) for enhanced code generation tooling
- [XmlSchemaClassGenerator](https://github.com/mganss/XmlSchemaClassGenerator) for the underlying code generation engine
- All contributors who help improve this library

## Support

- 📖 [Quick Start Guide](QUICKSTART.md)
- 🔧 [Schema Generation Guide](SCHEMA_GENERATION.md)
- 🐛 [Report Issues](https://github.com/Chizaruu/CargoWiseNetLibrary/issues)
- 💬 [Discussions](https://github.com/Chizaruu/CargoWiseNetLibrary/discussions)
- 📧 Contact: contact@koalahollow.com

---

**Author**: Chizaruu
Made with ❤️ for the logistics and freight forwarding community
