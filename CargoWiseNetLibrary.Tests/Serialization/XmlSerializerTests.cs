using CargoWiseNetLibrary.Serialization;
using FluentAssertions;
using Xunit;

namespace CargoWiseNetLibrary.Tests.Serialization;

public class XmlSerializerTests
{
    // Test model for serialization
    public class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    [Fact]
    public void Serialize_WithValidObject_ReturnsXmlString()
    {
        // Arrange
        var model = new TestModel
        {
            Name = "Test",
            Value = 42,
            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var xml = XmlSerializer<TestModel>.Serialize(model);

        // Assert
        xml.Should().NotBeNullOrEmpty();
        xml.Should().Contain("<TestModel");
        xml.Should().Contain("<Name>Test</Name>");
        xml.Should().Contain("<Value>42</Value>");
    }

    [Fact]
    public void Serialize_WithIndentOption_FormatsXml()
    {
        // Arrange
        var model = new TestModel { Name = "Test", Value = 42 };
        var options = new XmlSerializerOptions { Indent = true };

        // Act
        var xml = XmlSerializer<TestModel>.Serialize(model, options);

        // Assert
        xml.Should().Match(s => s.Contains("\r\n") || s.Contains('\n')); // Should have line breaks
    }

    [Fact]
    public void Serialize_WithNullObject_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => XmlSerializer<TestModel>.Serialize(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Deserialize_WithValidXml_ReturnsObject()
    {
        // Arrange
        var xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <TestModel>
                        <Name>Test</Name>
                        <Value>42</Value>
                        <CreatedAt>2025-01-01T00:00:00</CreatedAt>
                    </TestModel>";

        // Act
        var result = XmlSerializer<TestModel>.Deserialize(xml);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test");
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Deserialize_WithEmptyString_ReturnsNull()
    {
        // Act
        var result = XmlSerializer<TestModel>.Deserialize(string.Empty);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Deserialize_WithInvalidXml_ThrowsException()
    {
        // Arrange
        var invalidXml = "not xml at all";

        // Act & Assert
        var act = () => XmlSerializer<TestModel>.Deserialize(invalidXml);
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void SerializeToFile_CreatesFileWithXml()
    {
        // Arrange
        var model = new TestModel { Name = "FileTest", Value = 123 };
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            XmlSerializer<TestModel>.SerializeToFile(model, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            var content = File.ReadAllText(tempFile);
            content.Should().Contain("<Name>FileTest</Name>");
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task SerializeToFileAsync_CreatesFileWithXml()
    {
        // Arrange
        var model = new TestModel { Name = "AsyncTest", Value = 456 };
        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            await XmlSerializer<TestModel>.SerializeToFileAsync(model, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue();
            var content = await File.ReadAllTextAsync(tempFile);
            content.Should().Contain("<Name>AsyncTest</Name>");
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
            XmlSerializer<TestModel>.SerializeToFile(model, tempFile);

            // Act
            var result = XmlSerializer<TestModel>.DeserializeFromFile(tempFile);

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
        var nonExistentFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".xml");

        // Act & Assert
        var act = () => XmlSerializer<TestModel>.DeserializeFromFile(nonExistentFile);
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
            await XmlSerializer<TestModel>.SerializeToFileAsync(model, tempFile);

            // Act
            var result = await XmlSerializer<TestModel>.DeserializeFromFileAsync(tempFile);

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
    public void TryDeserialize_WithValidXml_ReturnsTrue()
    {
        // Arrange
        var xml = @"<TestModel><Name>Test</Name><Value>42</Value></TestModel>";

        // Act
        var success = XmlSerializer<TestModel>.TryDeserialize(xml, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test");
    }

    [Fact]
    public void TryDeserialize_WithInvalidXml_ReturnsFalse()
    {
        // Arrange
        var invalidXml = "not xml";

        // Act
        var success = XmlSerializer<TestModel>.TryDeserialize(invalidXml, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void IsValid_WithValidXml_ReturnsTrue()
    {
        // Arrange
        var xml = @"<TestModel><Name>Test</Name><Value>42</Value></TestModel>";

        // Act
        var isValid = XmlSerializer<TestModel>.IsValid(xml);

        // Assert
        isValid.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithInvalidXml_ReturnsFalse()
    {
        // Arrange
        var invalidXml = "not xml";

        // Act
        var isValid = XmlSerializer<TestModel>.IsValid(invalidXml);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void Clone_CreatesDeepCopy()
    {
        // Arrange
        var original = new TestModel
        {
            Name = "Original",
            Value = 100,
            CreatedAt = DateTime.Now
        };

        // Act
        var clone = XmlSerializer<TestModel>.Clone(original);

        // Assert
        clone.Should().NotBeNull();
        clone.Should().NotBeSameAs(original);
        clone.Name.Should().Be(original.Name);
        clone.Value.Should().Be(original.Value);
        clone.CreatedAt.Should().Be(original.CreatedAt);
    }

    [Fact]
    public void RoundTrip_PreservesData()
    {
        // Arrange
        var original = new TestModel
        {
            Name = "RoundTrip Test",
            Value = 12345,
            CreatedAt = new DateTime(2025, 11, 15, 0, 0, 0, DateTimeKind.Utc)
        };

        // Act
        var xml = XmlSerializer<TestModel>.Serialize(original);
        var deserialized = XmlSerializer<TestModel>.Deserialize(xml);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Name.Should().Be(original.Name);
        deserialized.Value.Should().Be(original.Value);
        deserialized.CreatedAt.Should().Be(original.CreatedAt);
    }
}
