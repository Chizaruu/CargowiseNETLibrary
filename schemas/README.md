# CargoWise XSD Schemas

This directory is used to store CargoWise XSD schema files that are used to generate the C# model classes.

## ⚠️ Important Legal Notice

**YOU MUST BE AN AUTHORIZED CARGOWISE USER OR AFFILIATED PARTNER** to access and use CargoWise XSD schemas.

- CargoWise® is a registered trademark of WiseTech Global Limited
- XSD schemas are proprietary to WiseTech Global Limited
- Only authorized CargoWise users can access these schemas via EDI Messaging
- See [LEGAL.md](../LEGAL.md) for complete legal information

## Directory Structure

```
schemas/
├── universal/         # CargoWise Universal schema XSD files
├── native/            # CargoWise Native schema XSD files
├── legal-header.txt   # Legal header template for generated files
├── generate-models.bat    # Windows generation script
├── generate-models.sh     # Linux/Mac generation script
└── README.md          # This file
```

## How to Obtain Schemas

### For Authorized CargoWise Users

1. **Log in to CargoWise** application
2. **Navigate to EDI Messaging** or your schema export area
3. **Export/Download XSD schemas** you need
4. **Save schemas** to the appropriate directory:
   - Universal schemas → `schemas/universal/`
   - Native schemas → `schemas/native/`

### Schema Files to Export

For a complete integration, you typically need:

#### Universal Schemas

- `UniversalShipment.xsd`
- `UniversalEvent.xsd`
- `UniversalInterchange.xsd`
- `UniversalResponse.xsd`
- `UniversalSchedule.xsd`
- `UniversalTransaction.xsd`
- Any related/dependent schema files (the tool will process all .xsd files in the directory)

#### Native Schemas (if needed)

- `Native.xsd` (main native schema)
- Any related/dependent schema files
- Consult CargoWise documentation for specific files

**Note**: The generation tools automatically process all `.xsd` files in the specified directories, so you don't need to specify individual files.

## Legal Header Template

The `legal-header.txt` file in this directory contains the legal notice template that will be automatically added to all generated model files. This ensures compliance and proper authorization notices.

**Template Location**: `schemas/legal-header.txt`

**Template Content**:

```
⚠️ IMPORTANT LEGAL NOTICE:
This file contains C# models generated from CargoWise XSD schemas.
The library maintainer is an authorized CargoWise user with schema access via EDI Messaging.

TO USE THIS LIBRARY:
- You MUST be an authorized CargoWise User or Affiliated Partner
- Having these models does NOT grant you permission to use CargoWise systems
- You MUST comply with all terms of your CargoWise user agreement
- XSD schemas are available to authorized users via CargoWise EDI Messaging
- See LEGAL.md for complete legal information

CargoWise® is a registered trademark of WiseTech Global Limited.
```

**Purpose**: Automatically adds legal notices to generated C# model files to inform users about authorization requirements.

## Generating C# Models

After placing XSD files in the appropriate directories:

### Option 1: Use the Batch Script (Windows) - RECOMMENDED

```cmd
# From the schemas directory
generate-models.bat
```

This automated script:

1. ✅ Generates models from all XSD files
2. ✅ Removes duplicate MessageNumberType declarations
3. ✅ Adds legal headers automatically
4. ✅ Merges duplicate class definitions
5. ✅ Cleans up file names
6. ✅ Updates README files with generation date

### Option 2: Use the Bash Script (Linux/Mac/WSL)

```bash
# From the schemas directory
./generate-models.sh
```

### Option 3: Manual Generation

**Prerequisites**:

```bash
# Install the tools if not already installed
dotnet tool install --global Chizaruu.NetTools.SchemaGenerator
dotnet tool install --global Chizaruu.NetTools.ClassMerger
```

**Generate Universal Models**:

```bash
schema-generator \
  --schema-dir ./schemas/universal \
  --output-dirs ./CargoWiseNetLibrary/Models \
  --namespaces CargoWiseNetLibrary.Models.Universal \
  --legal-header ./schemas/legal-header.txt \
  --duplicate-log ./Logs/universal_duplicates.log \
  --remove-duplicates MessageNumberType
```

**Generate Native Models**:

```bash
schema-generator \
  --schema-dir ./schemas/native \
  --output-dirs ./CargoWiseNetLibrary/Models \
  --namespaces CargoWiseNetLibrary.Models.Native \
  --legal-header ./schemas/legal-header.txt \
  --duplicate-log ./Logs/native_duplicates.log \
  --remove-duplicates MessageNumberType
```

**Merge Duplicate Classes** (Native.cs only):

```bash
class-merger ./CargoWiseNetLibrary/Models/Native.cs
```

**Key Parameters**:

- `--schema-dir` : Directory containing all XSD schema files
- `--output-dirs` : Output directory (models are consolidated into single files)
- `--namespaces` : C# namespace for generated classes
- `--legal-header` : Path to legal header template
- `--duplicate-log` : Path to log duplicate warnings
- `--remove-duplicates` : Types to remove duplicates of (prevents pre-generation errors)

**Note**: The tool generates **consolidated files** by default:

- All Universal models → `Universal.cs` (single file)
- All Native models → `Native.cs` (single file)

This is different from using `--separate-classes true`, which would create one file per type.

## What Gets Generated

The schema generator will:

- ✅ **Generate consolidated C# files** in `CargoWiseNetLibrary/Models/`
  - `Universal.cs` - All Universal schema models (57,406 lines)
  - `Native.cs` - All Native schema models (192,814 lines)
- ✅ **Add XML serialization attributes** to all classes
- ✅ **Add data validation annotations** (`[Required]`, `[MaxLength]`, etc.)
- ✅ **Handle nullable reference types** (.NET 8 compatibility)
- ✅ **Automatically add legal header notices** to generated files
- ✅ **Log duplicate type warnings** to specified log file
- ✅ **Pre-remove specified duplicates** (like MessageNumberType)

### File Organization

**Before cleanup**:

```
CargoWiseNetLibrary/Models/
├── CargoWiseNetLibrary.Models.Universal.cs    # Generated file
└── CargoWiseNetLibrary.Models.Native.cs       # Generated file
```

**After automated cleanup** (via batch script):

```
CargoWiseNetLibrary/Models/
├── Universal/
│   └── Universal.cs    # Renamed and organized
└── Native/
    └── Native.cs       # Renamed, organized, and merged
```

## Files in This Directory

**Note**: XSD schema files are `.gitignore`'d and will not be committed to the repository.

This is intentional because:

1. ✅ Schemas are proprietary to WiseTech Global Limited
2. ✅ Only authorized users should have access
3. ✅ Schemas may contain sensitive information
4. ✅ Reduces repository size
5. ✅ Prevents accidental distribution of proprietary schemas

**Files that ARE committed**:

- ✅ `legal-header.txt` - Legal notice template
- ✅ `generate-models.bat` - Windows generation script
- ✅ `generate-models.sh` - Linux/Mac generation script
- ✅ `README.md` - This file

## Troubleshooting

### "No .xsd files found"

**Solution**:

- Ensure you've placed `.xsd` files (lowercase extension) in the `schemas/universal/` or `schemas/native/` directory
- Check file extensions - they must be `.xsd` not `.XSD`
- Verify you're running the command from the correct directory

### "Permission denied" when exporting schemas

**Solution**: You must be an authorized CargoWise User or Affiliated Partner. Contact your CargoWise administrator for access.

### "Legal header file not found"

**Solution**:

- Ensure `legal-header.txt` exists in the schemas directory
- Check the path in your generation command
- The template should be in `schemas/legal-header.txt`

### Generated models don't compile

**Solution**:

- Ensure all dependent XSD files are in the same directory
- Check for invalid C# identifiers in schema type names
- Review the duplicate log in `Logs/` directory for issues
- Try running `class-merger` on the generated files

### Too many duplicate warnings

**Solution**:

- This is normal for CargoWise schemas - they contain many duplicate type definitions
- The generator handles duplicates gracefully
- Use `--duplicate-log` to save warnings to a file instead of cluttering console
- Use `--remove-duplicates` to pre-remove known problematic types

### Generation takes a very long time

**Solution**:

- Normal for CargoWise schemas - they're very large (100,000+ lines of output)
- Be patient - generation can take several minutes
- The batch script shows progress as it works
- Consider generating Universal and Native separately if needed

### MessageNumberType duplicate errors

**Solution**: This is why the scripts use `--remove-duplicates MessageNumberType` - it's a known duplicate type across schemas.

## Schema Versions

Keep track of which version of CargoWise schemas you're using:

**Create a version file**:

```bash
# Example version tracking
echo "CargoWise Schema Version: 2024.1" > schemas/VERSION.txt
echo "Export Date: $(date)" >> schemas/VERSION.txt
echo "Universal Schemas: 12 files" >> schemas/VERSION.txt
echo "Native Schemas: 8 files" >> schemas/VERSION.txt
```

**Document in comments**:

```csharp
// Generated from CargoWise schemas exported on 2024-11-16
// Schema version: 2024.1.0
// Total types: 5,247
```

## Best Practices

### 1. Version Control

- ✅ **DON'T** commit XSD files to git (they're in .gitignore)
- ✅ **DO** commit the legal-header.txt template
- ✅ **DO** commit generation scripts
- ✅ **DO** track schema versions in a VERSION.txt file

### 2. Legal Headers

- ✅ **ALWAYS** use `--legal-header` parameter when generating
- ✅ Verify legal headers are present in generated files
- ✅ Don't remove or modify legal headers

### 3. Documentation

- ✅ Keep notes on which schemas you've exported
- ✅ Document the CargoWise version you're using
- ✅ Note any custom modifications or exclusions

### 4. Backup

- ✅ Keep a backup of your schemas outside the repository
- ✅ Store schemas in a secure location
- ✅ Don't share schemas with unauthorized users

### 5. Regeneration

- ✅ Regenerate models when CargoWise releases schema updates
- ✅ Review diffs before committing regenerated models
- ✅ Test thoroughly after regeneration
- ✅ Update version tracking files

### 6. Testing

- ✅ Always run `dotnet build` after generation
- ✅ Run unit tests to verify models work correctly
- ✅ Test serialization/deserialization with sample data

### 7. Security

- ✅ Only store schemas locally on authorized systems
- ✅ Don't upload schemas to public repositories
- ✅ Respect CargoWise licensing terms
- ✅ Only share generated models with authorized partners

## Generation Process Flow

Here's what happens when you run the automated scripts:

```
1. Validation
   └─→ Check schema directories exist
   └─→ Verify .xsd files are present
   └─→ Validate legal-header.txt exists

2. Pre-Generation Deduplication
   └─→ Parse all XSD files
   └─→ Remove MessageNumberType duplicates
   └─→ Save to temporary directory

3. Code Generation (Universal)
   └─→ Process all universal/*.xsd files
   └─→ Generate CargoWiseNetLibrary.Models.Universal.cs
   └─→ Add legal headers
   └─→ Log duplicates to universal_duplicates.log

4. Code Generation (Native)
   └─→ Process all native/*.xsd files
   └─→ Generate CargoWiseNetLibrary.Models.Native.cs
   └─→ Add legal headers
   └─→ Log duplicates to native_duplicates.log

5. File Organization
   └─→ Rename files to Universal.cs and Native.cs
   └─→ Move to appropriate directories

6. Post-Processing (Native only)
   └─→ Run class-merger on Native.cs
   └─→ Merge duplicate Native/NativeBody classes
   └─→ Report any conflicts

7. Documentation Update
   └─→ Update README.md with generation date
   └─→ Update version badges
   └─→ Generate summary report
```

## Support

For questions about:

- **Obtaining schemas**: Contact your CargoWise administrator
- **Schema generator**: See [SCHEMA_GENERATION.md](../SCHEMA_GENERATION.md)
- **Legal questions**: See [LEGAL.md](../LEGAL.md)
- **Technical issues**: Open an issue on [GitHub](https://github.com/Chizaruu/CargoWiseNetLibrary/issues)
- **Chizaruu.NetTools**: See [Chizaruu.NetTools Repository](https://github.com/Chizaruu/Chizaruu.NetTools)

## Related Documentation

- [SCHEMA_GENERATION.md](../SCHEMA_GENERATION.md) - Detailed generation guide with examples
- [LEGAL.md](../LEGAL.md) - Legal information and disclaimers
- [CONTRIBUTING.md](../CONTRIBUTING.md) - Contribution guidelines
- [README.md](../README.md) - Main library documentation
- [QUICKSTART.md](../QUICKSTART.md) - Quick start guide

## Example: Complete Generation Workflow

Here's a complete example of obtaining schemas and generating models:

### Step 1: Obtain Schemas

```bash
# Log into CargoWise
# Navigate to: Setup > EDI Messaging > Schema Export
# Select: Universal schemas
# Download all .xsd files to: ./schemas/universal/
```

### Step 2: Verify Files

```bash
# Check universal schemas
ls -la schemas/universal/*.xsd
# Should show multiple .xsd files

# Check native schemas (if applicable)
ls -la schemas/native/*.xsd
```

### Step 3: Run Generation

```cmd
# Windows
cd schemas
generate-models.bat

# Wait for completion (may take 5-10 minutes)
```

### Step 4: Verify Output

```bash
# Check generated files
ls -la CargoWiseNetLibrary/Models/Universal/
ls -la CargoWiseNetLibrary/Models/Native/

# Verify legal headers are present
head -n 30 CargoWiseNetLibrary/Models/Universal/Universal.cs
```

### Step 5: Build and Test

```bash
# Build the solution
dotnet build

# Run tests
dotnet test

# Review any warnings or errors
```

### Step 6: Commit Changes

```bash
git add CargoWiseNetLibrary/Models/
git add README.md  # If updated with new date
git commit -m "Regenerate models from CargoWise schemas (2024-11-16)"
git push
```

---

**Remember**: You must be an authorized CargoWise User or Affiliated Partner to access and use these schemas. All generated models include legal notice headers to ensure compliance with licensing requirements.

**Questions?** Open an issue on GitHub or contact the maintainer at contact@koalahollow.com
