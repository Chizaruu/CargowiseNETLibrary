using System.Text.Json;
using CargoWiseNetLibrary.Serialization;
using FluentAssertions;
using Xunit;

namespace CargoWiseNetLibrary.Tests.Serialization;

public class JsonSerializerTests
{
    // Test model for serialization
    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string>? Tags { get; set; }
    }

    [Fact]
    public void Serialize_WithValidObject_ReturnsJsonString()
    {
        // Arrange
        var model = new TestModel
        {
            Name = "Test",
            Value = 42,
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Tags = ["tag1", "tag2"]
        };

        // Act
        var json = JsonSerializer<TestModel>.Serialize(model);

        // Assert
        json.Should().NotBeNullOrEmpty();
        // Fix: Use Match instead of .Or for conditional contains
        json.Should().Match(s => s.Contains("\"Name\"") || s.Contains("\"name\""));
        json.Should().Contain("Test");
        json.Should().Contain("42");
    }

    [Fact]
    public void Serialize_WithCustomOptions_UsesOptions()
    {
        // Arrange
        var model = new TestModel { Name = "Test", Value = 42 };
        var options = new JsonSerializerOptions { WriteIndented = false };

        // Act
        var json = JsonSerializer<TestModel>.Serialize(model, options);

        // Assert
        json.Should().NotContain("\n"); // Should not have line breaks
    }

    [Fact]
    public void Serialize_WithNullObject_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => JsonSerializer<TestModel>.Serialize(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deserialize_WithValidJson_ReturnsObject()
    {
        // Arrange
        var json = @"{
            ""Name"": ""Test"",
            ""Value"": 42,
            ""CreatedAt"": ""2025-01-01T00:00:00"",
            ""Tags"": [""tag1"", ""tag2""]
        }";

        // Act
        var result = JsonSerializer<TestModel>.Deserialize(json);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test");
        result.Value.Should().Be(42);
        result.Tags.Should().HaveCount(2);
    }

    [Fact]
    public void Deserialize_WithEmptyString_ReturnsNull()
    {
        // Act
        var result = JsonSerializer<TestModel>.Deserialize(string.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Deserialize_WithInvalidJson_ThrowsException()
    {
        // Arrange
        var invalidJson = "not json at all";

        // Act & Assert
        var act = () => JsonSerializer<TestModel>.Deserialize(invalidJson);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void SerializeToFile_CreatesFileWithJson()
    {
        // Arrange
        var model = new TestModel { Name = "FileTest", Value = 123 };
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            JsonSerializer<TestModel>.SerializeToFile(model, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            var content = File.ReadAllText(tempFile);
            content.Should().Contain("FileTest");
            content.Should().Contain("123");
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task SerializeToFileAsync_CreatesFileWithJson()
    {
        // Arrange
        var model = new TestModel { Name = "AsyncTest", Value = 456 };
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            await JsonSerializer<TestModel>.SerializeToFileAsync(model, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            var content = await File.ReadAllTextAsync(tempFile);
            content.Should().Contain("AsyncTest");
            content.Should().Contain("456");
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void DeserializeFromFile_ReturnsObject()
    {
        // Arrange
        var model = new TestModel { Name = "FileTest", Value = 789 };
        var tempFile = Path.GetTempFileName();

        try
        {
            JsonSerializer<TestModel>.SerializeToFile(model, tempFile);

            // Act
            var result = JsonSerializer<TestModel>.DeserializeFromFile(tempFile);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("FileTest");
            result.Value.Should().Be(789);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void DeserializeFromFile_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".json");

        // Act & Assert
        var act = () => JsonSerializer<TestModel>.DeserializeFromFile(nonExistentFile);
        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public async Task DeserializeFromFileAsync_ReturnsObject()
    {
        // Arrange
        var model = new TestModel { Name = "AsyncFileTest", Value = 999 };
        var tempFile = Path.GetTempFileName();

        try
        {
            await JsonSerializer<TestModel>.SerializeToFileAsync(model, tempFile);

            // Act
            var result = await JsonSerializer<TestModel>.DeserializeFromFileAsync(tempFile);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("AsyncFileTest");
            result.Value.Should().Be(999);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void Clone_CreatesDeepCopy()
    {
        // Arrange
        var original = new TestModel
        {
            Name = "Original",
            Value = 100,
            CreatedAt = DateTime.Now,
            Tags = ["test"]
        };

        // Act
        var clone = JsonSerializer<TestModel>.Clone(original);

        // Assert
        clone.Should().NotBeNull();
        clone.Should().NotBeSameAs(original);
        clone.Name.Should().Be(original.Name);
        clone.Value.Should().Be(original.Value);
        clone.Tags.Should().NotBeSameAs(original.Tags); // Deep copy
        clone.Tags.Should().BeEquivalentTo(original.Tags);
    }

    [Fact]
    public void Clone_WithNullObject_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => JsonSerializer<TestModel>.Clone(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TryDeserialize_WithValidJson_ReturnsTrue()
    {
        // Arrange
        var json = @"{""Name"":""Test"",""Value"":42}";

        // Act
        var success = JsonSerializer<TestModel>.TryDeserialize(json, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test");
        result.Value.Should().Be(42);
    }

    [Fact]
    public void TryDeserialize_WithInvalidJson_ReturnsFalse()
    {
        // Arrange
        var invalidJson = "not json";

        // Act
        var success = JsonSerializer<TestModel>.TryDeserialize(invalidJson, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void TryDeserialize_WithEmptyString_ReturnsFalse()
    {
        // Act
        var success = JsonSerializer<TestModel>.TryDeserialize(string.Empty, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void IsValid_WithValidJson_ReturnsTrue()
    {
        // Arrange
        var json = @"{""Name"":""Test"",""Value"":42}";

        // Act
        var isValid = JsonSerializer<TestModel>.IsValid(json);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithInvalidJson_ReturnsFalse()
    {
        // Arrange
        var invalidJson = "not json";

        // Act
        var isValid = JsonSerializer<TestModel>.IsValid(invalidJson);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void RoundTrip_PreservesData()
    {
        // Arrange
        var original = new TestModel
        {
            Name = "RoundTrip Test",
            Value = 12345,
            CreatedAt = new DateTime(2025, 11, 15, 0, 0, 0, DateTimeKind.Utc),
            Tags = ["important", "test"]
        };

        // Act
        var json = JsonSerializer<TestModel>.Serialize(original);
        var deserialized = JsonSerializer<TestModel>.Deserialize(json);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Name.Should().Be(original.Name);
        deserialized.Value.Should().Be(original.Value);
        deserialized.Tags.Should().BeEquivalentTo(original.Tags!);
    }

    [Fact]
    public async Task AsyncRoundTrip_PreservesData()
    {
        // Arrange
        var original = new TestModel
        {
            Name = "Async RoundTrip",
            Value = 54321,
            Tags = ["async", "test"]
        };
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            await JsonSerializer<TestModel>.SerializeToFileAsync(original, tempFile);
            var deserialized = await JsonSerializer<TestModel>.DeserializeFromFileAsync(tempFile);

            // Assert
            deserialized.Should().NotBeNull();
            deserialized!.Name.Should().Be(original.Name);
            deserialized.Value.Should().Be(original.Value);
            deserialized.Tags.Should().BeEquivalentTo(original.Tags!);
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void Serialize_WithNullProperties_OmitsNulls()
    {
        // Arrange
        var model = new TestModel
        {
            Name = "Test",
            Value = 42,
            Tags = null // Null property
        };

        // Act
        var json = JsonSerializer<TestModel>.Serialize(model);

        // Assert
        json.Should().NotContain("Tags"); // Nulls should be omitted
    }
}
