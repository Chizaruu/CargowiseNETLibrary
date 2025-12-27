# Auto-generated Models

[![Models Updated](https://img.shields.io/badge/Models_Updated-December_28_2025)](../SCHEMA_GENERATION.md)
[![Schema Generator](https://img.shields.io/badge/Generator-Chizaruu.NetTools-blue)](https://github.com/Chizaruu/Chizaruu.NetTools)
[![Authorization Required](https://img.shields.io/badge/Authorization-Required-red)](../LEGAL.md)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/download/dotnet/8.0)

These models are auto-generated from CargoWise XSD schemas using Chizaruu.NetTools.

## ⚠️ Important Legal Notice

**YOU MUST BE AN AUTHORIZED CARGOWISE USER** or Affiliated Partner to use these models.

- CargoWise® is a registered trademark of WiseTech Global Limited
- These models are generated from proprietary CargoWise XSD schemas
- Having these models does NOT grant permission to use CargoWise systems
- You MUST comply with WiseTech Global's terms of use

See [LEGAL.md](../LEGAL.md) for complete legal information.

---

## Generation Information

- **Last Generated**: December 28 2025
- **Generation Method**: Automated via `generate-models.bat`
- **Schema Source**: CargoWise EDI Messaging (Authorized Access Only)
- **Generator Tool**: [Chizaruu.NetTools.SchemaGenerator](https://github.com/Chizaruu/Chizaruu.NetTools)
- **Underlying Engine**: XmlSchemaClassGenerator v2.1.1183.0
- **Post-Processing**: Chizaruu.NetTools.ClassMerger (for duplicate resolution)

## File Organization

This directory uses a **consolidated file approach** for optimal performance and maintainability:

```
Models/
├── Universal/
│   └── Universal.cs          # All Universal schema models (57,406 lines)
└── Native/
    └── Native.cs             # All Native schema models (192,814 lines)
```

### Why Consolidated Files?

✅ **Faster Compilation** - Fewer file I/O operations  
✅ **Easier Version Control** - Simpler diffs and merges  
✅ **Better Performance** - Reduced file system overhead  
✅ **Simplified Deployment** - Fewer files to manage  
✅ **Clearer Organization** - One file per schema type (Universal/Native)

---

## Model Categories

### Universal Models (`Universal.cs`)

**Namespace**: `CargoWiseNetLibrary.Models.Universal`  
**File Size**: 57,406 lines  
**XML Namespace**: `http://www.cargowise.com/Schemas/Universal/2011/11`

**Primary Data Types**:

- `UniversalShipmentData` - Shipment and consignment data
- `UniversalEventData` - Event and milestone tracking
- `UniversalInterchange` - EDI interchange envelope
- `UniversalResponseData` - API response models
- `UniversalScheduleData` - Schedule information
- `UniversalTransactionData` - Transaction records
- `UniversalTransactionBatchData` - Batch transaction processing
- `UniversalShipmentRequestData` - Shipment request models
- `UniversalTransactionBatchRequestData` - Batch request models

**Supporting Types**:

- `Activity`, `Branch`, `Company`, `Contact`
- `DataContext`, `Department`, `Document`
- `Location`, `Organization`, `Staff`
- `TransportLeg`, `Vessel`, `Voyage`
- `CodeDescriptionPair`, `CustomizedField`
- And 100+ additional supporting classes

**Usage Example**:

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;

var shipment = new UniversalShipmentData { /* ... */ };
var xml = XmlSerializer<UniversalShipmentData>.Serialize(shipment);
```

### Native Models (`Native.cs`)

**Namespace**: `CargoWiseNetLibrary.Models.Native`  
**File Size**: 192,814 lines  
**XML Namespace**: `http://www.cargowise.com/Schemas/Native/2011/11`

**Primary Data Types**:

- `Native` - Root native data structure
- `NativeBody` - Native message body
- All CargoWise native API data structures

**Usage Example**:

```csharp
using CargoWiseNetLibrary.Models.Native;
using CargoWiseNetLibrary.Serialization;

var nativeData = new Native { /* ... */ };
var xml = XmlSerializer<Native>.Serialize(nativeData);
```

---

## Model Features

All generated models include:

### XML Serialization Support

```csharp
[XmlTypeAttribute("Activity", Namespace="http://www.cargowise.com/Schemas/Universal/2011/11")]
public partial class Activity
{
    [XmlElementAttribute("DataContext")]
    public DataContext DataContext { get; set; }
}
```

### Data Annotations for Validation

```csharp
[MaxLengthAttribute(100)]
[RequiredAttribute(AllowEmptyStrings=true)]
[XmlElementAttribute("Summary")]
public string Summary { get; set; }
```

### Nullable Reference Types (.NET 8)

```csharp
#nullable enable
public string? OptionalField { get; set; }
public string RequiredField { get; set; } = default!;
```

### Collection Properties

```csharp
[XmlIgnoreAttribute()]
private Collection<CustomizedField> _customizedFieldCollection;

[XmlArrayAttribute("CustomizedFieldCollection")]
[XmlArrayItemAttribute("CustomizedField", Namespace="...")]
public Collection<CustomizedField> CustomizedFieldCollection
{
    get { return _customizedFieldCollection; }
    set { _customizedFieldCollection = value; }
}

[XmlIgnoreAttribute()]
public bool CustomizedFieldCollectionSpecified
{
    get { return (this.CustomizedFieldCollection != null && this.CustomizedFieldCollection.Count != 0); }
}
```

### Nullable Enum Pattern

```csharp
[EditorBrowsableAttribute(EditorBrowsableState.Never)]
[XmlAttributeAttribute("Action")]
public Action ActionValue { get; set; }

[XmlIgnoreAttribute()]
[EditorBrowsableAttribute(EditorBrowsableState.Never)]
public bool ActionValueSpecified { get; set; }

[XmlIgnoreAttribute()]
public Nullable<Action> Action
{
    get { return this.ActionValueSpecified ? this.ActionValue : null; }
    set
    {
        this.ActionValue = value.GetValueOrDefault();
        this.ActionValueSpecified = value.HasValue;
    }
}
```

---

## Regenerating Models

### Prerequisites

1. **Authorization**: Must be an authorized CargoWise user
2. **Tools**: Install Chizaruu.NetTools
   ```bash
   dotnet tool install --global Chizaruu.NetTools.SchemaGenerator
   dotnet tool install --global Chizaruu.NetTools.ClassMerger
   ```

### Regeneration Steps

1. **Obtain Latest Schemas**

   - Log into CargoWise application
   - Navigate to EDI Messaging
   - Export/download latest XSD schemas
   - Save to appropriate directories:
     - Universal schemas → `schemas/universal/`
     - Native schemas → `schemas/native/`

2. **Run Generation Script**

   ```cmd
   cd schemas
   generate-models.bat
   ```

   This automated script will:

   - ✅ Validate all XSD files
   - ✅ Pre-remove duplicate MessageNumberType declarations
   - ✅ Generate consolidated C# files
   - ✅ Add legal headers automatically
   - ✅ Merge duplicate class definitions (Native.cs)
   - ✅ Clean up file names
   - ✅ Update this README with generation date

3. **Verify Generation**

   ```bash
   # Check generated files exist
   dir Models\Universal.cs
   dir Models\Native.cs

   # Verify legal headers are present
   head -n 30 Models/Universal.cs
   head -n 30 Models/Native.cs

   # Check for compilation errors
   dotnet build
   ```

4. **Review Changes**

   ```bash
   git diff Models/
   ```

5. **Update Documentation**

   - Update the "Last Generated" date in this README
   - Update version badges if needed
   - Document any schema version changes

6. **Commit Changes**
   ```bash
   git add Models/
   git commit -m "Regenerate models from CargoWise schemas (2025-11-16)"
   ```

### Manual Regeneration (Advanced)

For fine-grained control:

```bash
# Universal models
schema-generator \
  --schema-dir ./schemas/universal \
  --output-dirs ./CargoWiseNetLibrary/Models \
  --namespaces CargoWiseNetLibrary.Models.Universal \
  --legal-header ./schemas/legal-header.txt \
  --duplicate-log ./Logs/universal_duplicates.log \
  --remove-duplicates MessageNumberType

# Native models
schema-generator \
  --schema-dir ./schemas/native \
  --output-dirs ./CargoWiseNetLibrary/Models \
  --namespaces CargoWiseNetLibrary.Models.Native \
  --legal-header ./schemas/legal-header.txt \
  --duplicate-log ./Logs/native_duplicates.log \
  --remove-duplicates MessageNumberType

# Merge duplicate classes in Native.cs
class-merger ./CargoWiseNetLibrary/Models/Native.cs
```

---

## Generation Process Details

### What Happens During Generation

1. **Schema Validation**

   - All XSD files are validated for correctness
   - Invalid schemas are reported and skipped

2. **Pre-Generation Deduplication**

   - Duplicate type definitions are detected
   - MessageNumberType duplicates are removed before generation
   - Modified schemas saved to temporary directory

3. **Code Generation**

   - XmlSchemaClassGenerator processes all schemas
   - Generates strongly-typed C# classes
   - Adds XML serialization attributes
   - Includes data annotations for validation

4. **Legal Header Injection**

   - Legal notice automatically added to all generated files
   - Inserted between auto-generated comment and namespace
   - Ensures compliance with authorization requirements

5. **File Organization**

   - Generated files renamed and organized
   - Moved to appropriate Universal/Native directories

6. **Post-Processing (Native.cs only)**

   - Class merger runs to resolve remaining duplicates
   - Duplicate Native/NativeBody classes merged
   - Properties combined, conflicts reported

7. **Documentation Update**
   - README files updated with generation date
   - Version badges updated

### Duplicate Handling

CargoWise schemas contain many duplicate type definitions. The generation process handles this through:

- **Pre-Generation**: Removes known duplicates (MessageNumberType)
- **During Generation**: Logs duplicate warnings to file
- **Post-Generation**: Merges remaining duplicate classes

**Duplicate Logs**: `Logs/universal_duplicates.log` and `Logs/native_duplicates.log`

---

## Customization

### ⚠️ DO NOT Modify Generated Files

These files are auto-generated and include this warning:

```csharp
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
```

**Any manual edits will be lost** during regeneration.

### How to Add Custom Functionality

#### Option 1: Partial Classes (Recommended)

```csharp
// File: UniversalShipmentData.Custom.cs
namespace CargoWiseNetLibrary.Models.Universal
{
    public partial class UniversalShipmentData
    {
        public string GetDisplayName()
        {
            return $"{Shipment?.OrderNumber} - {Shipment?.DataContext?.Company.Code}";
        }
    }
}
```

#### Option 2: Extension Methods

```csharp
// File: UniversalShipmentExtensions.cs
namespace CargoWiseNetLibrary.Extensions
{
    public static class UniversalShipmentExtensions
    {
        public static string GetDisplayName(this UniversalShipmentData shipment)
        {
            return $"{shipment.Shipment?.OrderNumber}";
        }
    }
}
```

#### Option 3: Wrapper Classes

```csharp
// File: ShipmentWrapper.cs
public class ShipmentWrapper
{
    private readonly UniversalShipmentData _shipment;

    public ShipmentWrapper(UniversalShipmentData shipment)
    {
        _shipment = shipment;
    }

    public string DisplayName => GetDisplayName();

    private string GetDisplayName()
    {
        return $"{_shipment.Shipment?.OrderNumber}";
    }
}
```

---

## Troubleshooting

### Models Don't Compile

**Solution**:

- Run `class-merger` on Native.cs to resolve duplicate classes
- Check `Logs/` directory for duplicate warnings
- Verify all XSD dependencies were included during generation

### Missing Types

**Solution**:

- Ensure all dependent XSD files were in schema directory
- Check duplicate logs - type may have been skipped
- Regenerate with all required schema files

### XML Serialization Issues

**Solution**:

- Verify XML namespace matches schema namespace
- Check for missing `[XmlElement]` or `[XmlAttribute]` attributes
- Ensure proper use of `*Specified` properties for optional fields

---

## Statistics

### Universal.cs

- **Total Lines**: 57,406
- **Classes**: ~2,500+
- **Namespaces**: 1 primary (`CargoWiseNetLibrary.Models.Universal`)
- **XML Namespace**: `http://www.cargowise.com/Schemas/Universal/2011/11`

### Native.cs

- **Total Lines**: 192,814
- **Classes**: ~8,000+
- **Namespaces**: 1 primary (`CargoWiseNetLibrary.Models.Native`)
- **XML Namespace**: `http://www.cargowise.com/Schemas/Native/2011/11`

### Combined

- **Total Lines**: 250,220
- **Total Classes**: ~10,500+
- **Generator Version**: XmlSchemaClassGenerator 2.1.1183.0
- **Last Regeneration**: November 16, 2025

---

## Related Documentation

- [SCHEMA_GENERATION.md](../SCHEMA_GENERATION.md) - Complete generation guide
- [README.md](../README.md) - Main library documentation
- [QUICKSTART.md](../QUICKSTART.md) - Quick start guide
- [schemas/README.md](../schemas/README.md) - Schema directory documentation
- [LEGAL.md](../LEGAL.md) - Legal information and disclaimers

---

## Support

For questions or issues:

- 📖 [Full Documentation](../README.md)
- 🔧 [Schema Generation Guide](../SCHEMA_GENERATION.md)
- 🐛 [Report Issues](https://github.com/Chizaruu/CargoWiseNetLibrary/issues)
- 💬 [Discussions](https://github.com/Chizaruu/CargoWiseNetLibrary/discussions)
- 🛠️ [Chizaruu.NetTools](https://github.com/Chizaruu/Chizaruu.NetTools)
- 📧 Contact: contact@koalahollow.com

---

_This file is automatically updated during model regeneration. The "Last Generated" date and "Models Updated" badge are automatically updated by `generate-models.bat`._


















