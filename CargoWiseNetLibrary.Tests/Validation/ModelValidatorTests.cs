using System.ComponentModel.DataAnnotations;
using CargoWiseNetLibrary.Validation;
using FluentAssertions;
using Xunit;

namespace CargoWiseNetLibrary.Tests.Validation;

public class ModelValidatorTests
{
    // Test models for validation
    private class ValidTestModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Value { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }

    private class ComplexTestModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public List<string> Items { get; set; } = [];

        [Range(typeof(DateTime), "2020-01-01", "2030-12-31")]
        public DateTime ValidUntil { get; set; }
    }

    [Fact]
    public void Validate_WithValidModel_ReturnsValidResult()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel
        {
            Name = "Test",
            Value = 50,
            Email = "test@example.com"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithInvalidModel_ReturnsInvalidResult()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel
        {
            Name = "", // Required field empty
            Value = 150, // Out of range
            Email = "not-an-email" // Invalid email
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public void Validate_WithNullModel_ThrowsArgumentNullException()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();

        // Act & Assert
        var act = () => validator.Validate(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Validate_WithRequiredFieldMissing_HasError()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel
        {
            Name = "", // Required but empty
            Value = 50
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ValidTestModel.Name));
    }

    [Fact]
    public void Validate_WithRangeViolation_HasError()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel
        {
            Name = "Test",
            Value = 999 // Out of range (1-100)
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ValidTestModel.Value));
    }

    [Fact]
    public void Validate_WithInvalidEmail_HasError()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel
        {
            Name = "Test",
            Value = 50,
            Email = "not-an-email"
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ValidTestModel.Email));
    }

    [Fact]
    public async Task ValidateAsync_WithValidModel_ReturnsValidResult()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel
        {
            Name = "Test",
            Value = 50
        };

        // Act
        var result = await validator.ValidateAsync(model);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateMany_WithMultipleModels_ReturnsAllResults()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var models = new[]
        {
            new ValidTestModel { Name = "Valid1", Value = 50 },
            new ValidTestModel { Name = "", Value = 999 }, // Invalid
            new ValidTestModel { Name = "Valid2", Value = 25 }
        };

        // Act
        var results = validator.ValidateMany(models);

        // Assert
        results.Should().HaveCount(3);
        results.Values.Count(r => r.IsValid).Should().Be(2);
        results.Values.Count(r => !r.IsValid).Should().Be(1);
    }

    [Fact]
    public async Task ValidateManyAsync_WithMultipleModels_ReturnsAllResults()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var models = new[]
        {
            new ValidTestModel { Name = "Valid1", Value = 50 },
            new ValidTestModel { Name = "", Value = 999 }
        };

        // Act
        var results = await validator.ValidateManyAsync(models);

        // Assert
        results.Should().HaveCount(2);
    }

    [Fact]
    public void ValidateAll_WithAllValid_ReturnsTrue()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var models = new[]
        {
            new ValidTestModel { Name = "Valid1", Value = 50 },
            new ValidTestModel { Name = "Valid2", Value = 25 },
            new ValidTestModel { Name = "Valid3", Value = 75 }
        };

        // Act
        var allValid = validator.ValidateAll(models);

        // Assert
        allValid.Should().BeTrue();
    }

    [Fact]
    public void ValidateAll_WithOneInvalid_ReturnsFalse()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var models = new[]
        {
            new ValidTestModel { Name = "Valid1", Value = 50 },
            new ValidTestModel { Name = "", Value = 999 }, // Invalid
            new ValidTestModel { Name = "Valid2", Value = 25 }
        };

        // Act
        var allValid = validator.ValidateAll(models);

        // Assert
        allValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateAllAsync_WithAllValid_ReturnsTrue()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var models = new[]
        {
            new ValidTestModel { Name = "Valid1", Value = 50 },
            new ValidTestModel { Name = "Valid2", Value = 25 }
        };

        // Act
        var allValid = await validator.ValidateAllAsync(models);

        // Assert
        allValid.Should().BeTrue();
    }

    [Fact]
    public void TryValidate_WithValidModel_ReturnsTrue()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel { Name = "Test", Value = 50 };

        // Act
        var success = validator.TryValidate(model, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.IsValid.Should().BeTrue();
    }

    [Fact]
    public void TryValidate_WithNullModel_ReturnsFalse()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();

        // Act
        var success = validator.TryValidate(null, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void GetAllErrors_ReturnsAllErrorsFromCollection()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var models = new[]
        {
            new ValidTestModel { Name = "", Value = 999 }, // 2-3 errors
            new ValidTestModel { Name = "Valid", Value = 50 }, // 0 errors
            new ValidTestModel { Name = "", Value = 200 } // 2-3 errors
        };

        // Act
        var allErrors = validator.GetAllErrors(models);

        // Assert
        allErrors.Should().NotBeEmpty();
        allErrors.Count.Should().BeGreaterThan(2);
    }

    [Fact]
    public void GetInvalidCount_ReturnsCorrectCount()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var models = new[]
        {
            new ValidTestModel { Name = "Valid1", Value = 50 },
            new ValidTestModel { Name = "", Value = 999 }, // Invalid
            new ValidTestModel { Name = "Valid2", Value = 25 },
            new ValidTestModel { Name = "", Value = 200 } // Invalid
        };

        // Act
        var invalidCount = validator.GetInvalidCount(models);

        // Assert
        invalidCount.Should().Be(2);
    }

    [Fact]
    public void GetValidModels_FiltersToOnlyValidModels()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var validModel1 = new ValidTestModel { Name = "Valid1", Value = 50 };
        var invalidModel = new ValidTestModel { Name = "", Value = 999 };
        var validModel2 = new ValidTestModel { Name = "Valid2", Value = 25 };

        var models = new[] { validModel1, invalidModel, validModel2 };

        // Act
        var validModels = validator.GetValidModels(models).ToList();

        // Assert
        validModels.Should().HaveCount(2);
        validModels.Should().Contain(validModel1);
        validModels.Should().Contain(validModel2);
        validModels.Should().NotContain(invalidModel);
    }

    [Fact]
    public void GetInvalidModels_FiltersToOnlyInvalidModels()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var validModel = new ValidTestModel { Name = "Valid1", Value = 50 };
        var invalidModel1 = new ValidTestModel { Name = "", Value = 999 };
        var invalidModel2 = new ValidTestModel { Name = "", Value = 200 };

        var models = new[] { validModel, invalidModel1, invalidModel2 };

        // Act
        var invalidModels = validator.GetInvalidModels(models).ToList();

        // Assert
        invalidModels.Should().HaveCount(2);
        invalidModels.Should().Contain(invalidModel1);
        invalidModels.Should().Contain(invalidModel2);
        invalidModels.Should().NotContain(validModel);
    }

    [Fact]
    public void Validate_WithComplexModel_ValidatesAllProperties()
    {
        // Arrange
        var validator = new ModelValidator<ComplexTestModel>();
        var model = new ComplexTestModel
        {
            Id = "test-id",
            Items = ["item1", "item2"],
            ValidUntil = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.Should().NotBeNull();
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithComplexModel_DetectsMultipleErrors()
    {
        // Arrange
        var validator = new ModelValidator<ComplexTestModel>();
        var model = new ComplexTestModel
        {
            Id = "", // Required violation
            Items = ["only-one"], // MinLength violation
            ValidUntil = new DateTime(2035, 1, 1, 0, 0, 0, DateTimeKind.Utc) // Range violation
        };

        // Act
        var result = validator.Validate(model);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(3);
    }

    [Fact]
    public void ValidationError_ContainsPropertyName()
    {
        // Arrange
        var validator = new ModelValidator<ValidTestModel>();
        var model = new ValidTestModel { Name = "", Value = 50 };

        // Act
        var result = validator.Validate(model);

        // Assert
        var error = result.Errors[0];
        error.PropertyName.Should().NotBeNullOrEmpty();
        error.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}
