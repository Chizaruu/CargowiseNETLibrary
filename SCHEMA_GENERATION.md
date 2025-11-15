# Schema Generation Guide

## Overview

All C# models in this library are generated from CargoWise XSD schemas using **[Chizaruu.NetTools](https://github.com/Chizaruu/Chizaruu.NetTools)**, a custom .NET tool suite built on top of XmlSchemaClassGenerator that adds features specifically designed for CargoWise schema processing.

The library uses a **consolidated file approach**, generating all Universal models into a single `Universal.cs` file (57,406 lines) and all Native models into a single `Native.cs` file (192,814 lines).

---

## Tool Stack

### Primary Generation Tool: Chizaruu.NetTools

**Repository**: https://github.com/Chizaruu/Chizaruu.NetTools  
**Built On**: XmlSchemaClassGenerator v2.1.1183.0  
**License**: MIT

**Chizaruu.NetTools provides two key tools**:

1. **schema-generator** - Enhanced XSD-to-C# code generator

   - ✅ Built on XmlSchemaClassGenerator
   - ✅ Automatic legal header injection
   - ✅ Pre-generation duplicate type removal
   - ✅ Multi-namespace support
   - ✅ Consolidated file generation
   - ✅ Duplicate warning logging
   - ✅ Intelligent error handling

2. **class-merger** - Post-processing tool
   - ✅ Merges duplicate class definitions
   - ✅ Combines properties from multiple definitions
   - ✅ Handles constructor merging
   - ✅ Conflict detection and reporting

### Underlying Technology

**XmlSchemaClassGenerator**: Industry-standard XSD-to-C# code generation library that provides the core transformation functionality

---

## Installation

### Install Chizaruu.NetTools

```bash
# Install as a global .NET tool
dotnet tool install --global Chizaruu.NetTools.SchemaGenerator
dotnet tool install --global Chizaruu.NetTools.ClassMerger
```

### Verify Installation

```bash
# Check schema-generator
schema-generator --help

# Check class-merger
class-merger --help
```

---

## File Organization

The library uses a **consolidated file approach**:

```
CargoWiseNetLibrary/
└── Models/
    ├── Universal/
    │   └── Universal.cs       # All Universal models (57,406 lines)
    └── Native/
        └── Native.cs           # All Native models (192,814 lines)
```

**Benefits of Consolidated Approach**:

- ✅ Easier to manage and version control
- ✅ Faster compilation (fewer file I/O operations)
- ✅ Simpler deployment
- ✅ Clearer namespace organization
- ✅ Reduced file system overhead

---

## Generation Process

### Step 1: Obtain CargoWise Schemas

**For Authorized Users Only**:

1. Log in to CargoWise application
2. Navigate to **EDI Messaging**
3. Export/download XSD schema files
4. Save to schema directories:
   - Universal schemas → `schemas/universal/`
   - Native schemas → `schemas/native/`

See [schemas/README.md](schemas/README.md) for detailed instructions.

### Step 2: Generate Models

#### Option 1: Using the Automated Script (Recommended)

**Windows**:

```cmd
generate-models.bat
```

**Linux/Mac/WSL**:

```bash
./generate-models.sh
```

The script automatically:

1. Generates models from XSD schemas
2. Adds legal headers to all files
3. Merges duplicate class definitions
4. Cleans up file names
5. Updates README files with generation date

#### Option 2: Manual Generation

**Generate Universal Models**:

```bash
schema-generator \
  --schema-dir ./schemas/universal \
  --output-dirs ./CargoWiseNetLibrary/Models/Universal \
  --namespaces CargoWiseNetLibrary.Models.Universal \
  --legal-header ./schemas/legal-header.txt \
  --duplicate-log ./Logs/universal_duplicates.log \
  --remove-duplicates MessageNumberType
```

**Generate Native Models**:

```bash
schema-generator \
  --schema-dir ./schemas/native \
  --output-dirs ./CargoWiseNetLibrary/Models/Native \
  --namespaces CargoWiseNetLibrary.Models.Native \
  --legal-header ./schemas/legal-header.txt \
  --duplicate-log ./Logs/native_duplicates.log \
  --remove-duplicates MessageNumberType
```

**Key Parameters**:

- `--schema-dir` : Directory containing XSD schema files
- `--output-dirs` : Output directory for generated C# files
- `--namespaces` : C# namespace for generated classes
- `--legal-header` : Path to legal header template file
- `--duplicate-log` : Path to write duplicate warnings
- `--remove-duplicates` : Comma-separated list of types to deduplicate

### Step 3: Post-Generation Processing

After generation, the scripts perform:

1. **Legal Header Injection**

   - Adds required legal notices to all generated files
   - Ensures compliance with authorization requirements

2. **Duplicate Class Merging** (Native.cs only)

   - Merges duplicate class definitions (e.g., Native/NativeBody)
   - Resolves conflicts from overlapping schemas

3. **File Name Cleanup**

   - Removes namespace prefixes from file names
   - Ensures consistent naming

4. **Documentation Updates**
   - Updates README.md with generation date
   - Updates version badges

---

## Generated Code Structure

### File Header

Every generated file includes:

```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// This code was generated by XmlSchemaClassGenerator version 2.1.1183.0

// ============================================================================
// LEGAL NOTICE
// ============================================================================
// ⚠️ IMPORTANT LEGAL NOTICE:
// This file contains C# models generated from CargoWise XSD schemas.
// The library maintainer is an authorized CargoWise user with schema access via EDI Messaging.
//
// TO USE THIS LIBRARY:
// - You MUST be an authorized CargoWise User or Affiliated Partner
// - Having these models does NOT grant you permission to use CargoWise systems
// - You MUST comply with all terms of your CargoWise user agreement
// - XSD schemas are available to authorized users via CargoWise EDI Messaging
// - See LEGAL.md for complete legal information
//
// CargoWise® is a registered trademark of WiseTech Global Limited.
// ============================================================================

namespace CargoWiseNetLibrary.Models.Universal
{
    // ... generated classes
}
```

### Class Structure

Generated classes include:

**XML Serialization Attributes**:

```csharp
[GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.1.1183.0")]
[SerializableAttribute()]
[XmlTypeAttribute("Activity", Namespace="http://www.cargowise.com/Schemas/Universal/2011/11")]
public partial class Activity
{
    [XmlElementAttribute("DataContext")]
    public DataContext DataContext { get; set; }

    [MaxLengthAttribute(100)]
    [RequiredAttribute(AllowEmptyStrings=true)]
    [XmlElementAttribute("Summary")]
    public string Summary { get; set; }
}
```

**Collection Properties**:

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

**Nullable Enums**:

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
    get
    {
        if (this.ActionValueSpecified)
            return this.ActionValue;
        else
            return null;
    }
    set
    {
        this.ActionValue = value.GetValueOrDefault();
        this.ActionValueSpecified = value.HasValue;
    }
}
```

---

## Automated Generation Script Details

The `generate-models.bat` script performs the following:

### 1. Setup

```batch
REM Set paths
set "UNIVERSAL_DIR=schemas\universal"
set "NATIVE_DIR=schemas\native"
set "OUTPUT_DIR=CargoWiseNetLibrary\Models"
set "LOG_DIR=Logs"
set "LEGAL_HEADER=schemas\legal-header.txt"
```

### 2. Clean Old Files

```batch
REM Clean old generated files before regenerating
del /Q "%OUTPUT_DIR%\*.cs" 2>nul
```

### 3. Generate Universal Models

```batch
schema-generator ^
  --schema-dir "%UNIVERSAL_DIR%" ^
  --output-dirs "%OUTPUT_DIR%" ^
  --namespaces "CargoWiseNetLibrary.Models.Universal" ^
  --duplicate-log "%LOG_DIR%\universal_duplicates.log" ^
  --legal-header "%LEGAL_HEADER%" ^
  --remove-duplicates "MessageNumberType"
```

### 4. Generate Native Models

```batch
schema-generator ^
  --schema-dir "%NATIVE_DIR%" ^
  --output-dirs "%OUTPUT_DIR%" ^
  --namespaces "CargoWiseNetLibrary.Models.Native" ^
  --duplicate-log "%LOG_DIR%\native_duplicates.log" ^
  --legal-header "%LEGAL_HEADER%" ^
  --remove-duplicates "MessageNumberType"
```

### 5. Merge Duplicate Classes

```batch
REM Merge duplicate classes in Native.cs (Native/NativeBody)
class-merger "%OUTPUT_DIR%\Native.cs"
```

### 6. Update Documentation

```batch
REM Update README files with generation date
powershell -Command "Update badge and date in README files"
```

---

## schema-generator Features in Detail

### Legal Header Injection

The `--legal-header` parameter automatically injects legal notices into all generated files:

1. **Template Loading**: Loads template from specified file
2. **Smart Insertion**: Inserts between auto-generated block and namespace declaration
3. **Formatting**: Properly formats as C# comments with visual separators
4. **Class Detection**: Extracts class name to customize header

**Template Format**:

```
⚠️ IMPORTANT LEGAL NOTICE:
This file contains C# models generated from CargoWise XSD schemas.
...
```

**Inserted As**:

```csharp
// ============================================================================
// LEGAL NOTICE
// ============================================================================
// ⚠️ IMPORTANT LEGAL NOTICE:
// This file contains C# models generated from CargoWise XSD schemas.
// ...
// ============================================================================
```

### Pre-Generation Deduplication

The `--remove-duplicates` parameter preprocesses schemas to remove duplicate type definitions BEFORE code generation:

**How it Works**:

1. Parses all XSD files
2. Identifies duplicate `<simpleType>` and `<complexType>` declarations
3. Keeps first occurrence, removes subsequent duplicates
4. Saves modified schemas to temporary directory
5. Generates code from deduplicated schemas

**Example**:

```bash
--remove-duplicates MessageNumberType,AddressType
```

This prevents compilation errors from duplicate class definitions.

### Intelligent Error Handling

**Combined vs Individual Processing**:

1. **First Attempt**: Tries to process all schemas together (faster)
2. **Fallback**: If duplicates detected, processes schemas individually
3. **Result**: Maximum code generation with minimal errors

**Duplicate Logging**:

- All duplicate warnings logged to console
- Optionally saved to file with `--duplicate-log`
- Detailed reporting of which types conflicted

### Multi-Namespace Support

Process multiple schema sets in one command:

```bash
schema-generator \
  --schema-dir ./schemas/universal,./schemas/native \
  --output-dirs ./Models/Universal,./Models/Native \
  --namespaces MyApp.Models.Universal,MyApp.Models.Native
```

Each schema directory maps to corresponding output directory and namespace.

---

## class-merger Features

The `class-merger` tool performs post-generation processing to resolve remaining duplicates:

### Duplicate Class Detection

Scans generated files for classes with identical names and merges them intelligently.

### Property Merging

- Combines properties from all class definitions
- Preserves all unique properties
- Detects and reports conflicts
- Maintains attributes and documentation

### Constructor Handling

- Merges initialization code from multiple constructors
- Combines collection initializations
- Preserves all necessary initialization logic

### Usage

```bash
# Merge duplicates in a single file
class-merger Native.cs

# Merge with output to different file
class-merger Native.cs --output Native.merged.cs

# Process entire directory recursively
class-merger ./Models --recursive
```

---

## Handling Duplicates

CargoWise schemas contain duplicate type definitions. The generation process handles this through:

### Pre-Generation Filtering

Certain duplicates are removed before generation:

```batch
--remove-duplicates "MessageNumberType"
```

### Duplicate Logging

Duplicate warnings are logged to files:

- `Logs/universal_duplicates.log`
- `Logs/native_duplicates.log`

**Example Log**:

```
Duplicate Types Log - 2025-11-16 10:30:00
Total duplicates ignored: 5
----------------------------------------------
[CargoWiseNetLibrary.Models.Universal] Schema: CommonTypes.xsd, Warning: Type 'AddressType' already declared
[CargoWiseNetLibrary.Models.Universal] Schema: ShipmentTypes.xsd, Warning: Type 'CodeType' already declared
```

### Post-Generation Merging

The `class-merger` tool merges remaining duplicates:

- Detects duplicate class definitions
- Merges properties from all definitions
- Removes duplicate declarations
- Preserves all unique properties

**Example**: Native.cs contains both `Native` and `NativeBody` classes that may have duplicates. The merger combines them into single class definitions.

---

## Regenerating Models

### When to Regenerate

Regenerate models when:

- CargoWise releases schema updates
- You need additional schema types
- Bug fixes in generation tools
- Upgrading XmlSchemaClassGenerator version

### Regeneration Steps

1. **Backup Current Models**

   ```bash
   cp -r CargoWiseNetLibrary/Models CargoWiseNetLibrary/Models.backup
   ```

2. **Export Updated Schemas**

   - Log into CargoWise
   - Navigate to EDI Messaging
   - Export latest XSD files to `schemas/` directory

3. **Run Generation Script**

   ```cmd
   generate-models.bat
   ```

4. **Review Changes**

   ```bash
   # Compare with backup
   diff -r CargoWiseNetLibrary/Models.backup CargoWiseNetLibrary/Models

   # Or use git
   git diff CargoWiseNetLibrary/Models/
   ```

5. **Test Compilation**

   ```bash
   dotnet build
   dotnet test
   ```

6. **Commit Changes**
   ```bash
   git add CargoWiseNetLibrary/Models/
   git commit -m "Regenerate models from updated CargoWise schemas"
   ```

---

## schema-generator Command Reference

### Basic Usage

```bash
schema-generator --schema-dir <directory> --output-dirs <directory> [options]
```

### Required Options

| Option          | Description                                                 | Example               |
| --------------- | ----------------------------------------------------------- | --------------------- |
| `--schema-dir`  | Directory(ies) containing XSD files (comma-separated)       | `./schemas/universal` |
| `--output-dirs` | Output directory(ies) for generated files (comma-separated) | `./Models/Universal`  |

### Chizaruu.NetTools Specific Options

| Option                | Description                                                       | Example                         |
| --------------------- | ----------------------------------------------------------------- | ------------------------------- |
| `--namespaces`        | C# namespaces for generated classes (comma-separated)             | `MyApp.Models.Universal`        |
| `--legal-header`      | Path to legal header template file                                | `./schemas/legal-header.txt`    |
| `--duplicate-log`     | Path to write duplicate warnings log                              | `./Logs/duplicates.log`         |
| `--remove-duplicates` | Comma-separated list of types to deduplicate                      | `MessageNumberType,AnotherType` |
| `--separate-classes`  | Generate separate file per type (default: false for consolidated) | N/A                             |

### Advanced Features

**Multi-Namespace Generation**:

```bash
schema-generator \
  --schema-dir ./schemas/universal,./schemas/native \
  --output-dirs ./Models/Universal,./Models/Native \
  --namespaces MyApp.Models.Universal,MyApp.Models.Native \
  --legal-header ./schemas/legal-header.txt
```

**Pre-Generation Deduplication**:

```bash
schema-generator \
  --schema-dir ./schemas \
  --output-dirs ./Models \
  --namespaces MyApp.Models \
  --remove-duplicates MessageNumberType,AddressType,CodeType
```

This removes duplicate type definitions BEFORE code generation, preventing compilation errors.

---

## Troubleshooting

### Issue: "No .xsd files found"

**Cause**: Schema files not in expected location or wrong extension  
**Solution**:

- Ensure `.xsd` files (lowercase) are in `schemas/universal/` or `schemas/native/`
- Check file extensions are `.xsd` not `.XSD`

### Issue: Compilation errors after generation

**Cause**: Duplicate or conflicting type definitions  
**Solution**:

- Review duplicate logs in `Logs/` directory
- Run `class-merger` on affected files
- Check for namespace conflicts

### Issue: Missing types in generated code

**Cause**: XSD imports/includes not resolved  
**Solution**:

- Ensure all dependent XSD files are in same directory
- Check XSD import/include paths are relative
- Verify all referenced schemas are exported from CargoWise

### Issue: "Permission denied" when exporting schemas

**Cause**: Not an authorized CargoWise user  
**Solution**: You must be an authorized CargoWise User or Affiliated Partner

### Issue: Legal headers not added

**Cause**: `legal-header.txt` not found or not specified  
**Solution**:

- Verify `schemas/legal-header.txt` exists
- Use `--legal-header` parameter in generation command

### Issue: Generation takes very long

**Cause**: Large number of XSD files with complex dependencies  
**Solution**:

- Normal for CargoWise schemas (thousands of types)
- Be patient - generation can take several minutes
- Consider generating Universal and Native separately

---

## Best Practices

### Version Control

✅ **DO commit**:

- Generated model files (`Universal.cs`, `Native.cs`)
- Legal header template (`schemas/legal-header.txt`)
- Generation scripts (`generate-models.bat`, `generate-models.sh`)
- Documentation

❌ **DON'T commit**:

- XSD schema files (`.gitignore`'d - proprietary)
- Duplicate log files (`Logs/*.log`)
- Backup directories

### Regeneration

1. **Always backup** before regenerating
2. **Review diffs** carefully after regeneration
3. **Test thoroughly** before committing
4. **Document changes** in commit message
5. **Update README** with generation date

### Schema Management

1. **Track schema versions** - Keep notes on which CargoWise version
2. **Document changes** - Note what changed between schema versions
3. **Test incrementally** - Don't update all schemas at once
4. **Keep backups** - Store XSD files outside repository

---

## Manual Customization

### ⚠️ Warning: Don't Edit Generated Files

Generated files include this warning:

```csharp
// <auto-generated>
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
```

**Any manual edits will be lost** when models are regenerated.

### Alternatives to Manual Editing

If you need custom functionality:

**Option 1: Partial Classes**

Create separate partial class files:

```csharp
// File: UniversalShipmentData.Custom.cs
namespace CargoWiseNetLibrary.Models.Universal
{
    public partial class UniversalShipmentData
    {
        // Add custom methods, properties, etc.
        public string GetDisplayName()
        {
            return $"{Shipment?.OrderNumber} - {Shipment?.DataContext?.CompanyCode}";
        }
    }
}
```

**Option 2: Extension Methods**

```csharp
// File: UniversalShipmentExtensions.cs
namespace CargoWiseNetLibrary.Extensions
{
    public static class UniversalShipmentExtensions
    {
        public static string GetDisplayName(this UniversalShipmentData shipment)
        {
            return $"{shipment.Shipment?.OrderNumber} - {shipment.Shipment?.DataContext?.CompanyCode}";
        }
    }
}
```

**Option 3: Wrapper Classes**

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

    // Custom logic here
}
```

---

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Regenerate CargoWise Models

on:
  workflow_dispatch:
    inputs:
      schema_version:
        description: "CargoWise schema version"
        required: true

jobs:
  regenerate:
    runs-on: windows-latest

    steps:
      - name: Checkout
        uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: 8.0.x

      - name: Install Generation Tools
        run: |
          dotnet tool install --global Chizaruu.NetTools.SchemaGenerator
          dotnet tool install --global Chizaruu.NetTools.ClassMerger

      - name: Download Schemas
        run: |
          # Your authenticated schema download script
          # (requires CargoWise credentials)
        env:
          CARGOWISE_USER: ${{ secrets.CARGOWISE_USER }}
          CARGOWISE_PASS: ${{ secrets.CARGOWISE_PASS }}

      - name: Generate Models
        run: ./generate-models.bat

      - name: Build and Test
        run: |
          dotnet build
          dotnet test

      - name: Create Pull Request
        uses: peter-evans/create-pull-request@v5
        with:
          title: "Update CargoWise models to ${{ github.event.inputs.schema_version }}"
          body: |
            Automated model regeneration from CargoWise schemas.

            Schema Version: ${{ github.event.inputs.schema_version }}
            Generated: ${{ github.run_number }}
          branch: update-models-${{ github.run_number }}
```

---

## Additional Resources

### Documentation

- [Chizaruu.NetTools Repository](https://github.com/Chizaruu/Chizaruu.NetTools)
- [XmlSchemaClassGenerator Documentation](https://github.com/mganss/XmlSchemaClassGenerator)
- [XML Schema Specification](https://www.w3.org/TR/xmlschema-0/)
- [CargoWise Documentation](https://www.cargowise.com/) (authorized users only)

### Tools

- [Chizaruu.NetTools on GitHub](https://github.com/Chizaruu/Chizaruu.NetTools)
- [XmlSchemaClassGenerator on GitHub](https://github.com/mganss/XmlSchemaClassGenerator)
- [Visual Studio XML Tools](https://visualstudio.microsoft.com/)
- [Online XSD Validators](https://www.freeformatter.com/xml-validator-xsd.html)

### Related Files

- [schemas/README.md](schemas/README.md) - How to obtain schemas
- [LEGAL.md](LEGAL.md) - Legal information and disclaimers
- [README.md](README.md) - Main library documentation

---

## Credits

- **Chizaruu.NetTools**: Created by Chizaruu - https://github.com/Chizaruu/Chizaruu.NetTools
- **XmlSchemaClassGenerator**: Michael Ganss and contributors - underlying code generation engine
- **CargoWise**: WiseTech Global Limited - schema provider
- **Library Author**: Chizaruu

---

**Important**: This library and its generation process are independent of CargoWise and WiseTech Global. Users must be authorized CargoWise Users or Affiliated Partners to access and use CargoWise XSD schemas.
