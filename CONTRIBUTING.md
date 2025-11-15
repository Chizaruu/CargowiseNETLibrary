# Contributing to CargoWiseNetLibrary

First off, thank you for considering contributing to CargoWiseNetLibrary! It's people like you that make this library a great tool for the logistics and freight forwarding community.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How Can I Contribute?](#how-can-i-contribute)
- [Development Setup](#development-setup)
- [Project Structure](#project-structure)
- [Coding Guidelines](#coding-guidelines)
- [Working with Generated Models](#working-with-generated-models)
- [Testing Guidelines](#testing-guidelines)
- [Documentation](#documentation)
- [Pull Request Process](#pull-request-process)
- [Legal Considerations](#legal-considerations)

---

## Code of Conduct

This project and everyone participating in it is governed by our Code of Conduct. By participating, you are expected to uphold this code.

### Our Pledge

We pledge to make participation in our project a harassment-free experience for everyone, regardless of:

- Age, body size, disability, ethnicity, gender identity and expression
- Level of experience, education, socio-economic status
- Nationality, personal appearance, race, religion
- Sexual identity and orientation

### Expected Behavior

- ✅ Be respectful, inclusive, and professional in all interactions
- ✅ Welcome newcomers and help them get started
- ✅ Give and receive constructive feedback gracefully
- ✅ Focus on what is best for the community
- ✅ Show empathy towards other community members

### Unacceptable Behavior

- ❌ Harassment, trolling, or derogatory comments
- ❌ Personal or political attacks
- ❌ Publishing others' private information
- ❌ Other conduct which could reasonably be considered inappropriate

---

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the [existing issues](https://github.com/Chizaruu/CargoWiseNetLibrary/issues) to see if the problem has already been reported.

**When creating a bug report, include**:

1. **Clear and descriptive title**

   - Good: "XmlSerializer throws NullReferenceException with empty UniversalInterchange"
   - Bad: "Serialization broken"

2. **Steps to reproduce**

   ```csharp
   // Minimal code example that reproduces the issue
   var interchange = new UniversalInterchange();
   var xml = XmlSerializer<UniversalInterchange>.Serialize(interchange);
   // Exception thrown here
   ```

3. **Expected behavior**

   - What you expected to happen

4. **Actual behavior**

   - What actually happened
   - Include full error messages and stack traces

5. **Environment details**

   - .NET version: `dotnet --version`
   - OS: Windows/Linux/macOS version
   - Library version: Check NuGet package version
   - CargoWise schema version (if known)

6. **Sample data** (if applicable)
   - Sanitized XML/JSON samples
   - Remove any sensitive or proprietary information

### Suggesting Enhancements

Enhancement suggestions are tracked as [GitHub issues](https://github.com/Chizaruu/CargoWiseNetLibrary/issues).

**When creating an enhancement suggestion, include**:

1. **Clear and descriptive title**
2. **Detailed description** of the suggested enhancement
3. **Use cases** - Explain why this would be useful
4. **Examples** - Show how it would be used
5. **Alternatives considered** - What other solutions did you consider?
6. **Backward compatibility** - Will this break existing code?

**Example Enhancement Request**:

````markdown
## Enhancement: Add batch validation with parallel processing

**Use Case**: Validating 10,000+ shipments takes too long with current sequential validation.

**Proposed Solution**: Add `ValidateManyAsync` with parallel processing option.

**Example Usage**:

```csharp
var validator = new ModelValidator<UniversalShipmentData>();
var results = await validator.ValidateManyAsync(shipments,
    new ValidationOptions { MaxDegreeOfParallelism = 4 });
```
````

**Backward Compatibility**: New method, no breaking changes.

````

### Improving Documentation

Documentation improvements are always welcome!

- Fix typos or unclear explanations
- Add missing code examples
- Improve API documentation comments
- Add troubleshooting guides
- Create tutorials or how-to guides

### Pull Requests

We love pull requests! To increase the chances of your PR being accepted:

1. ✅ Follow the [coding guidelines](#coding-guidelines)
2. ✅ Include appropriate test cases
3. ✅ Update documentation as needed
4. ✅ Ensure all tests pass
5. ✅ Use clear commit messages
6. ✅ Keep changes focused and atomic
7. ❌ Do not modify generated model files (they will be regenerated)

---

## Development Setup

### Prerequisites

1. **.NET 8.0 SDK**
   ```bash
   # Download from https://dotnet.microsoft.com/download/dotnet/8.0

   # Verify installation
   dotnet --version
   # Should show 8.0.x
````

2. **Git**

   ```bash
   git --version
   ```

3. **IDE** (recommended)

   - Visual Studio 2022 (Community or higher)
   - JetBrains Rider
   - Visual Studio Code with C# extension

4. **Chizaruu.NetTools** (for model generation)
   ```bash
   dotnet tool install --global Chizaruu.NetTools.SchemaGenerator
   dotnet tool install --global Chizaruu.NetTools.ClassMerger
   ```

### Initial Setup

1. **Fork the repository**

   - Visit https://github.com/Chizaruu/CargoWiseNetLibrary
   - Click "Fork" button

2. **Clone your fork**

   ```bash
   git clone https://github.com/yourusername/CargoWiseNetLibrary.git
   cd CargoWiseNetLibrary
   ```

3. **Add upstream remote**

   ```bash
   git remote add upstream https://github.com/Chizaruu/CargoWiseNetLibrary.git
   ```

4. **Build the solution**

   ```bash
   dotnet build
   ```

5. **Run tests** (when available)

   ```bash
   dotnet test
   ```

6. **Verify everything works**
   ```bash
   dotnet build --configuration Release
   ```

---

## Project Structure

```
CargoWiseNetLibrary/
├── CargoWiseNetLibrary/                    # Main library project
│   ├── Models/
│   │   ├── Universal/
│   │   │   └── Universal.cs                # All Universal models (57,406 lines) - AUTO-GENERATED
│   │   └── Native/
│   │       └── Native.cs                   # All Native models (192,814 lines) - AUTO-GENERATED
│   ├── Serialization/                      # Serialization helpers
│   │   ├── XmlSerializer.cs                # XML serialization utilities
│   │   ├── JsonSerializer.cs               # JSON serialization utilities
│   │   └── UniversalInterchangeHelper.cs   # UniversalInterchange utilities
│   └── Validation/                         # Validation utilities
│       ├── ModelValidator.cs               # Validation framework
│       └── ValidationResult.cs             # Validation result models
├── CargoWiseNetLibrary.Tests/              # Unit tests (to be created)
├── schemas/                                # XSD schemas (not committed)
│   ├── universal/                          # Universal schema files
│   ├── native/                             # Native schema files
│   ├── legal-header.txt                    # Legal header template
│   └── generate-models.bat                 # Generation script
├── docs/                                   # Documentation
├── README.md                               # Main documentation
├── QUICKSTART.md                           # Quick start guide
├── SCHEMA_GENERATION.md                    # Schema generation guide
├── LEGAL.md                                # Legal information
└── CONTRIBUTING.md                         # This file
```

### File Organization Notes

- **Consolidated Models**: All Universal models are in a single `Universal.cs` file, all Native models in `Native.cs`
- **Auto-Generated**: Model files are auto-generated - DO NOT EDIT manually
- **Schemas Not Committed**: XSD files are `.gitignore`'d as they're proprietary to WiseTech Global

---

## Coding Guidelines

### C# Style

Follow [Microsoft's C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions):

#### General Principles

```csharp
// ✅ Good: Clear, descriptive names
public ValidationResult ValidateShipment(UniversalShipmentData shipment)
{
    ArgumentNullException.ThrowIfNull(shipment);
    // Implementation
}

// ❌ Bad: Unclear, abbreviated names
public VR VS(USD s)
{
    if (s == null) throw new Exception();
    // Implementation
}
```

#### Naming Conventions

| Type             | Convention  | Example                 |
| ---------------- | ----------- | ----------------------- |
| Classes          | PascalCase  | `UniversalShipmentData` |
| Methods          | PascalCase  | `ValidateAsync`         |
| Properties       | PascalCase  | `IsValid`               |
| Fields (private) | \_camelCase | `_validator`            |
| Parameters       | camelCase   | `shipmentData`          |
| Local variables  | camelCase   | `validationResult`      |
| Constants        | PascalCase  | `MaxRetryCount`         |

#### Code Style Preferences

```csharp
// ✅ Use var when type is obvious
var shipment = new UniversalShipmentData();
var xml = XmlSerializer<UniversalShipmentData>.Serialize(shipment);

// ✅ Explicit type when not obvious
XmlSerializerOptions options = GetDefaultOptions();

// ✅ Use nullable reference types
public string? OptionalField { get; set; }
public string RequiredField { get; set; } = default!;

// ✅ Use expression-bodied members for simple cases
public bool IsValid => Errors.Count == 0;
public string GetDisplayName() => $"{FirstName} {LastName}";

// ✅ Use pattern matching
if (obj is UniversalShipmentData shipment)
{
    ProcessShipment(shipment);
}

// ✅ Use async/await properly
public async Task<ValidationResult> ValidateAsync(UniversalShipmentData shipment)
{
    ArgumentNullException.ThrowIfNull(shipment);

    await Task.CompletedTask; // Placeholder
    return new ValidationResult { IsValid = true };
}
```

### XML Documentation

All public APIs must have XML documentation:

```csharp
/// <summary>
/// Validates the specified model using Data Annotations.
/// </summary>
/// <typeparam name="T">The type of model to validate.</typeparam>
/// <param name="model">The model to validate.</param>
/// <returns>
/// A <see cref="ValidationResult"/> indicating whether the model is valid
/// and containing any validation errors found.
/// </returns>
/// <exception cref="ArgumentNullException">
/// Thrown when <paramref name="model"/> is null.
/// </exception>
/// <example>
/// <code>
/// var validator = new ModelValidator&lt;UniversalShipmentData&gt;();
/// var result = validator.Validate(shipment);
/// if (!result.IsValid)
/// {
///     foreach (var error in result.Errors)
///     {
///         Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
///     }
/// }
/// </code>
/// </example>
public ValidationResult Validate(T model)
{
    ArgumentNullException.ThrowIfNull(model);

    // Implementation
}
```

### Error Handling

```csharp
// ✅ Use ArgumentNullException.ThrowIfNull
public void ProcessShipment(UniversalShipmentData shipment)
{
    ArgumentNullException.ThrowIfNull(shipment);
    // Process...
}

// ✅ Use ArgumentException for validation
public void SetMaxLength(int length)
{
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);
    // Set value...
}

// ✅ Provide helpful error messages
throw new InvalidOperationException(
    "Cannot serialize shipment with empty DataContext. " +
    "Ensure DataContext is populated before serialization.");

// ❌ Avoid generic exceptions
throw new Exception("Something went wrong");
```

---

## Working with Generated Models

### ⚠️ CRITICAL: DO NOT Edit Generated Models

Files in `CargoWiseNetLibrary/Models/Universal/Universal.cs` and `CargoWiseNetLibrary/Models/Native/Native.cs` are **AUTO-GENERATED**.

**Why you shouldn't edit them**:

1. ❌ Changes will be lost when models are regenerated
2. ❌ Breaks synchronization with CargoWise schemas
3. ❌ May cause serialization/deserialization issues
4. ❌ Makes maintenance and updates difficult

**Every generated file includes this warning**:

```csharp
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
```

### How to Extend Generated Models

#### Option 1: Partial Classes (Recommended)

Create separate files with partial classes:

```csharp
// File: UniversalShipmentData.Extensions.cs
namespace CargoWiseNetLibrary.Models.Universal
{
    /// <summary>
    /// Extended functionality for UniversalShipmentData.
    /// </summary>
    public partial class UniversalShipmentData
    {
        /// <summary>
        /// Gets a display-friendly name for the shipment.
        /// </summary>
        public string GetDisplayName()
        {
            return $"{Shipment?.OrderNumber} - {Shipment?.DataContext?.CompanyCode}";
        }

        /// <summary>
        /// Validates that the shipment has required context information.
        /// </summary>
        public bool HasValidContext()
        {
            return Shipment?.DataContext != null
                && !string.IsNullOrEmpty(Shipment.DataContext.CompanyCode);
        }
    }
}
```

#### Option 2: Extension Methods

```csharp
// File: UniversalShipmentExtensions.cs
namespace CargoWiseNetLibrary.Extensions
{
    /// <summary>
    /// Extension methods for UniversalShipmentData.
    /// </summary>
    public static class UniversalShipmentExtensions
    {
        /// <summary>
        /// Gets a display-friendly name for the shipment.
        /// </summary>
        public static string GetDisplayName(this UniversalShipmentData shipment)
        {
            ArgumentNullException.ThrowIfNull(shipment);
            return $"{shipment.Shipment?.OrderNumber} - {shipment.Shipment?.DataContext?.CompanyCode}";
        }
    }
}
```

#### Option 3: Wrapper Classes

```csharp
// File: ShipmentWrapper.cs
namespace CargoWiseNetLibrary.Wrappers
{
    /// <summary>
    /// Wrapper for UniversalShipmentData with additional functionality.
    /// </summary>
    public class ShipmentWrapper
    {
        private readonly UniversalShipmentData _shipment;

        public ShipmentWrapper(UniversalShipmentData shipment)
        {
            ArgumentNullException.ThrowIfNull(shipment);
            _shipment = shipment;
        }

        public string DisplayName => GetDisplayName();

        public bool IsValid => HasValidContext();

        private string GetDisplayName()
        {
            return $"{_shipment.Shipment?.OrderNumber}";
        }

        private bool HasValidContext()
        {
            return _shipment.Shipment?.DataContext != null;
        }
    }
}
```

### Regenerating Models

**When to regenerate**:

- CargoWise releases schema updates
- You need additional schema types
- Bug fixes in Chizaruu.NetTools

**How to regenerate**:

1. **Obtain latest XSD schemas** from CargoWise EDI Messaging (must be authorized user)

2. **Place XSD files** in appropriate directory:

   ```bash
   schemas/universal/    # For Universal schemas
   schemas/native/       # For Native schemas
   ```

3. **Run generation script** (recommended):

   ```cmd
   cd schemas
   generate-models.bat
   ```

   This script automatically:

   - Validates schemas
   - Removes duplicate MessageNumberType
   - Generates consolidated C# files
   - Adds legal headers
   - Merges duplicate classes
   - Updates documentation

4. **Or generate manually**:

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

   # Merge duplicate classes
   class-merger ./CargoWiseNetLibrary/Models/Native.cs
   ```

5. **Verify generation**:

   ```bash
   # Check files exist
   ls CargoWiseNetLibrary/Models/Universal.cs
   ls CargoWiseNetLibrary/Models/Native.cs

   # Build to check for errors
   dotnet build

   # Review changes
   git diff CargoWiseNetLibrary/Models/
   ```

6. **Update documentation**:
   - Update "Models Updated" badge in README.md
   - Update "Last Generated" date in Models/README.md
   - Document any breaking changes
   - Update changelog

**See [SCHEMA_GENERATION.md](SCHEMA_GENERATION.md) for detailed instructions.**

---

## Testing Guidelines

### Test Framework

- Use **xUnit** for test framework (when tests are added)
- Use **FluentAssertions** for readable assertions (optional)
- Follow **Arrange-Act-Assert** pattern

### What to Test

#### Serialization Tests

```csharp
[Fact]
public void Serialize_ValidShipment_ReturnsXmlString()
{
    // Arrange
    var shipment = new UniversalShipmentData
    {
        Shipment = new Shipment
        {
            DataContext = new DataContext
            {
                CompanyCode = "TEST",
                EnterpriseID = "TEST_ENT"
            }
        }
    };

    // Act
    var result = XmlSerializer<UniversalShipmentData>.Serialize(shipment);

    // Assert
    Assert.NotNull(result);
    Assert.Contains("UniversalShipment", result);
    Assert.Contains("TEST_ENT", result);
}

[Fact]
public void Deserialize_ValidXml_ReturnsShipment()
{
    // Arrange
    var xml = @"<?xml version=""1.0""?>
<UniversalShipment xmlns=""http://www.cargowise.com/Schemas/Universal/2011/11"">
  <Shipment>
    <DataContext>
      <CompanyCode>TEST</CompanyCode>
    </DataContext>
  </Shipment>
</UniversalShipment>";

    // Act
    var result = XmlSerializer<UniversalShipmentData>.Deserialize(xml);

    // Assert
    Assert.NotNull(result);
    Assert.NotNull(result.Shipment);
    Assert.Equal("TEST", result.Shipment.DataContext?.CompanyCode);
}
```

#### Validation Tests

```csharp
[Fact]
public void Validate_InvalidModel_ReturnsErrors()
{
    // Arrange
    var shipment = new UniversalShipmentData(); // Empty, invalid
    var validator = new ModelValidator<UniversalShipmentData>();

    // Act
    var result = validator.Validate(shipment);

    // Assert
    Assert.False(result.IsValid);
    Assert.NotEmpty(result.Errors);
}

[Theory]
[InlineData(null)]
public void Validate_NullModel_ThrowsArgumentNullException(UniversalShipmentData model)
{
    // Arrange
    var validator = new ModelValidator<UniversalShipmentData>();

    // Act & Assert
    Assert.Throws<ArgumentNullException>(() => validator.Validate(model));
}
```

#### Helper Methods Tests

```csharp
[Fact]
public void TryDeserialize_InvalidXml_ReturnsFalse()
{
    // Arrange
    var invalidXml = "<invalid>xml";

    // Act
    var success = XmlSerializer<UniversalShipmentData>.TryDeserialize(invalidXml, out var result);

    // Assert
    Assert.False(success);
    Assert.Null(result);
}

[Fact]
public void Clone_ValidObject_CreatesDeepCopy()
{
    // Arrange
    var original = new UniversalShipmentData
    {
        Shipment = new Shipment { OrderNumber = "TEST-001" }
    };

    // Act
    var clone = XmlSerializer<UniversalShipmentData>.Clone(original);
    clone.Shipment.OrderNumber = "TEST-002";

    // Assert
    Assert.NotEqual(original.Shipment.OrderNumber, clone.Shipment.OrderNumber);
    Assert.Equal("TEST-001", original.Shipment.OrderNumber);
    Assert.Equal("TEST-002", clone.Shipment.OrderNumber);
}
```

### What NOT to Test

- ❌ Generated model properties (they're auto-generated from schemas)
- ❌ .NET framework functionality
- ❌ Third-party library internals (XmlSchemaClassGenerator, System.Text.Json)

### Test Naming Convention

Use descriptive test names that explain what's being tested:

```
MethodName_Scenario_ExpectedBehavior

Examples:
- Serialize_ValidShipment_ReturnsXmlString
- Validate_NullModel_ThrowsArgumentNullException
- DeserializeFromFile_NonexistentFile_ThrowsFileNotFoundException
```

### Code Coverage Goals

- **Target**: 80%+ code coverage
- **Serialization**: 90%+ (critical functionality)
- **Validation**: 85%+ (important for data integrity)
- **Helpers**: 80%+ (utility methods)

---

## Documentation

### Types of Documentation

1. **XML Documentation Comments** - All public APIs
2. **README.md** - Main library documentation
3. **QUICKSTART.md** - Getting started guide
4. **SCHEMA_GENERATION.md** - Model generation guide
5. **LEGAL.md** - Legal information
6. **CONTRIBUTING.md** - This file

### Writing Good Documentation

#### XML Documentation Comments

```csharp
/// <summary>
/// Validates a collection of models asynchronously.
/// </summary>
/// <typeparam name="T">The type of models to validate.</typeparam>
/// <param name="models">The collection of models to validate.</param>
/// <param name="cancellationToken">
/// A cancellation token that can be used to cancel the validation.
/// </param>
/// <returns>
/// A task that represents the asynchronous operation. The task result contains
/// a dictionary mapping each model to its validation result.
/// </returns>
/// <exception cref="ArgumentNullException">
/// Thrown when <paramref name="models"/> is null.
/// </exception>
/// <remarks>
/// This method validates each model independently. Cancellation will stop
/// processing remaining models but already completed validations will be returned.
/// </remarks>
/// <example>
/// <code>
/// var validator = new ModelValidator&lt;UniversalShipmentData&gt;();
/// var models = GetShipments();
/// var results = await validator.ValidateManyAsync(models, cancellationToken);
///
/// foreach (var (model, result) in results)
/// {
///     if (!result.IsValid)
///     {
///         LogErrors(model, result.Errors);
///     }
/// }
/// </code>
/// </example>
public async Task<Dictionary<T, ValidationResult>> ValidateManyAsync(
    IEnumerable<T> models,
    CancellationToken cancellationToken = default)
{
    // Implementation
}
```

#### Code Examples

Always include runnable code examples:

````markdown
### Batch Processing Example

```csharp
using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using CargoWiseNetLibrary.Validation;

var files = Directory.GetFiles("imports", "*.xml");
var validator = new ModelValidator<UniversalShipmentData>();
var validShipments = new List<UniversalShipmentData>();

foreach (var file in files)
{
    if (XmlSerializer<UniversalShipmentData>.TryDeserialize(
        File.ReadAllText(file), out var shipment))
    {
        var result = validator.Validate(shipment);
        if (result.IsValid)
        {
            validShipments.Add(shipment);
        }
    }
}

Console.WriteLine($"Processed {validShipments.Count} valid shipments");
```
````

````

---

## Pull Request Process

### Before Submitting

1. ✅ **Update your fork** with latest changes from main
   ```bash
   git fetch upstream
   git checkout main
   git merge upstream/main
````

2. ✅ **Create a feature branch**

   ```bash
   git checkout -b feature/add-streaming-support
   ```

3. ✅ **Make your changes** following the coding guidelines

4. ✅ **Write/update tests** for your changes

5. ✅ **Update documentation** as needed

6. ✅ **Run all tests** and ensure they pass

   ```bash
   dotnet build
   dotnet test
   ```

7. ✅ **Commit with clear messages**

   ```bash
   git add .
   git commit -m "Add streaming support for large XML files"
   ```

8. ✅ **Push to your fork**
   ```bash
   git push origin feature/add-streaming-support
   ```

### Submitting the PR

1. Go to the [repository](https://github.com/Chizaruu/CargoWiseNetLibrary)
2. Click "New Pull Request"
3. Select your fork and branch
4. Fill in the PR template:

```markdown
## Description

Brief description of what this PR does.

## Motivation and Context

Why is this change required? What problem does it solve?

## Type of Change

- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## How Has This Been Tested?

Describe the tests you ran to verify your changes.

## Checklist

- [ ] My code follows the code style of this project
- [ ] I have updated the documentation accordingly
- [ ] I have added tests to cover my changes
- [ ] All new and existing tests passed
- [ ] My changes generate no new warnings
```

### Code Review

The maintainers will:

- Review within 1-2 weeks
- Provide constructive feedback
- Request changes if needed
- Merge approved contributions

**Respond to feedback promptly** and make requested changes.

---

## Legal Considerations

### CargoWise Authorization

**IMPORTANT**: Before contributing, ensure you understand:

1. ⚠️ **You MUST be an authorized CargoWise User or Affiliated Partner**
2. ⚠️ **XSD schemas are proprietary to WiseTech Global Limited**
3. ✅ **Contributions must respect intellectual property rights**
4. ✅ **Do not commit XSD schema files** (they're .gitignore'd)
5. ✅ **Maintain legal notices in generated files**

See [LEGAL.md](LEGAL.md) for complete legal information.

### Contributor License

By contributing, you agree that:

1. Your contributions will be licensed under **Apache License 2.0**
2. You have the right to submit the contributions
3. You grant the project maintainers a perpetual, worldwide, non-exclusive, royalty-free license
4. Your contributions do not violate any third-party rights

### Don't Commit Sensitive Data

Never commit:

- ❌ CargoWise credentials or API keys
- ❌ Production data or real customer information
- ❌ Personal information
- ❌ CargoWise XSD schema files (they're proprietary)

---

## Getting Help

### Resources

- 📖 [README.md](README.md) - Main documentation
- 📖 [QUICKSTART.md](QUICKSTART.md) - Getting started examples
- 📖 [SCHEMA_GENERATION.md](SCHEMA_GENERATION.md) - Model generation guide
- 📖 [LEGAL.md](LEGAL.md) - Legal information
- 🛠️ [Chizaruu.NetTools](https://github.com/Chizaruu/Chizaruu.NetTools) - Code generation tools

### Support Channels

- 🐛 [GitHub Issues](https://github.com/Chizaruu/CargoWiseNetLibrary/issues) - Bug reports and feature requests
- 💬 [GitHub Discussions](https://github.com/Chizaruu/CargoWiseNetLibrary/discussions) - Questions and discussions
- 📧 Email: contact@koalahollow.com - Direct contact with maintainers

### Quick Answers

**Q: Can I add support for a new CargoWise schema?**  
A: Yes! See [Working with Generated Models](#working-with-generated-models) section.

**Q: Can I modify the generated model files?**  
A: No, use partial classes or extension methods instead.

**Q: How do I test my changes?**  
A: Write unit tests following the [Testing Guidelines](#testing-guidelines).

**Q: What if my PR is rejected?**  
A: Work with maintainers to address concerns. Most rejections can be resolved through discussion and changes.

---

## Recognition

Contributors will be recognized in:

- Project README
- Release notes
- CONTRIBUTORS file (if created)
- GitHub contributor stats

### Hall of Fame

Contributors who make significant impacts will be specially recognized for:

- Major features
- Extensive documentation
- Ongoing maintenance
- Community support

---

## Commit Message Guidelines

### Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting, no logic change)
- **refactor**: Code refactoring
- **test**: Adding or updating tests
- **chore**: Maintenance tasks

### Examples

```
feat(serialization): Add streaming support for large XML files

- Implement XmlStreamReader for memory-efficient parsing
- Add StreamToFileAsync method
- Include comprehensive tests for streaming scenarios

Closes #42
```

```
fix(validation): Handle null collections in ValidateMany

Fixed NullReferenceException when ValidateMany receives a
collection containing null elements.

Fixes #56
```

```
docs(quickstart): Add UniversalInterchange examples

Added comprehensive examples showing:
- DeserializeAndExtract usage
- CreateInterchange wrapper
- Extension method examples
```

---

## Thank You! 🎉

Your contributions help make CargoWiseNetLibrary better for the entire logistics and freight forwarding community!

We appreciate:

- 🐛 Bug reports that help us improve
- 💡 Feature suggestions that drive innovation
- 📝 Documentation improvements that help users
- 🔧 Code contributions that add functionality
- 💬 Community support that helps others

**Together, we're building better tools for logistics!**

---

**Maintainer**: Chizaruu (Abdul-Kadir Coskun) / Koala Hollow  
**Repository**: https://github.com/Chizaruu/CargoWiseNetLibrary  
**License**: Apache 2.0  
**Contact**: contact@koalahollow.com
