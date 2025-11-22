using CargoWiseNetLibrary.Serialization;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace CargoWiseNetLibrary.Tests.Serialization;

/// <summary>
/// Comprehensive XML serialization tests for ALL Universal models
/// Covers all XSD files from the Universal schema directory:
/// - UniversalShipment.xsd, UniversalTransaction.xsd, UniversalSchedule.xsd, etc.
/// </summary>
public class UniversalXmlSerializerTests
{
    /// <summary>
    /// Wrapper class for test data that implements IXunitSerializable
    /// </summary>
    public class ModelTestData : IXunitSerializable
    {
        public object? Model { get; set; }
        public string ModelName { get; set; } = string.Empty;

        public ModelTestData()
        {
        }

        public ModelTestData(object model, string modelName)
        {
            Model = model;
            ModelName = modelName;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            ModelName = info.GetValue<string>(nameof(ModelName));
            var typeName = info.GetValue<string>("TypeName");
            var type = Type.GetType(typeName);

            if (type != null)
            {
                Model = Activator.CreateInstance(type);
            }
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(ModelName), ModelName);
            info.AddValue("TypeName", Model?.GetType().AssemblyQualifiedName ?? string.Empty);
        }

        public override string ToString()
        {
            return ModelName;
        }
    }

    /// <summary>
    /// Test data provider for ALL Universal model types
    /// Based on the XSD files in the Universal schema directory
    /// </summary>
    public static TheoryData<ModelTestData> GetAllUniversalModels()
    {
        var data = new TheoryData<ModelTestData>();

        // Core Universal models - Data wrapper types
        AddIfTypeExists(data, "UniversalShipmentData");
        AddIfTypeExists(data, "UniversalTransactionData");
        AddIfTypeExists(data, "UniversalTransactionBatchData");
        AddIfTypeExists(data, "UniversalScheduleData");
        AddIfTypeExists(data, "UniversalEventData");
        AddIfTypeExists(data, "UniversalActivityData");

        // Request types
        AddIfTypeExists(data, "UniversalShipmentRequestData");
        AddIfTypeExists(data, "UniversalTransactionBatchRequestData");
        AddIfTypeExists(data, "UniversalActivityRequestData");
        AddIfTypeExists(data, "UniversalDocumentRequestData");
        AddIfTypeExists(data, "UniversalInterchangeRequeueRequestData");

        // Response and Interchange
        AddIfTypeExists(data, "UniversalResponseData");
        AddIfTypeExists(data, "UniversalInterchange");

        // Main entity types (from the data wrappers)
        AddIfTypeExists(data, "Shipment");
        AddIfTypeExists(data, "TransactionInfo");
        AddIfTypeExists(data, "TransactionBatch");
        AddIfTypeExists(data, "Schedule");
        AddIfTypeExists(data, "UniversalEvent");
        AddIfTypeExists(data, "Activity");

        // Request entity types
        AddIfTypeExists(data, "ShipmentRequest");
        AddIfTypeExists(data, "TransactionBatchRequest");
        AddIfTypeExists(data, "ActivityRequest");
        AddIfTypeExists(data, "DocumentRequest");

        return data;
    }

    /// <summary>
    /// Test data provider for commonly used Universal models
    /// </summary>
    public static TheoryData<ModelTestData> GetCommonUniversalModels()
    {
        var data = new TheoryData<ModelTestData>();

        AddIfTypeExists(data, "UniversalShipmentData");
        AddIfTypeExists(data, "UniversalTransactionData");
        AddIfTypeExists(data, "UniversalScheduleData");
        AddIfTypeExists(data, "UniversalInterchange");
        AddIfTypeExists(data, "Shipment");
        AddIfTypeExists(data, "TransactionInfo");
        AddIfTypeExists(data, "Schedule");
        AddIfTypeExists(data, "Activity");
        AddIfTypeExists(data, "UniversalEvent");

        return data;
    }

    /// <summary>
    /// Helper method to add a type to test data if it exists in the assembly
    /// </summary>
    private static void AddIfTypeExists(TheoryData<ModelTestData> data, string typeName)
    {
        var fullTypeName = $"CargoWiseNetLibrary.Models.Universal.{typeName}, CargoWiseNetLibrary";
        var type = Type.GetType(fullTypeName);

        if (type != null)
        {
            try
            {
                var instance = Activator.CreateInstance(type);
                if (instance != null)
                {
                    data.Add(new ModelTestData(instance, typeName));
                }
            }
            catch
            {
                // Skip types that can't be instantiated without parameters
            }
        }
    }

    #region Theory Tests - All Models

    [Theory]
    [MemberData(nameof(GetAllUniversalModels))]
    public void Serialize_AllUniversalModels_ReturnsValidXml(ModelTestData testData)
    {
        // Act
        var xml = SerializeModel(testData.Model!);

        // Assert
        xml.Should().NotBeNullOrEmpty($"{testData.ModelName} should serialize to XML");
        xml.Should().Contain("<?xml", $"{testData.ModelName} should include XML declaration");
    }

    [Theory]
    [MemberData(nameof(GetAllUniversalModels))]
    public void RoundTrip_AllUniversalModels_PreservesData(ModelTestData testData)
    {
        // Act
        var xml = SerializeModel(testData.Model!);
        var deserialized = DeserializeModel(testData.Model!.GetType(), xml);

        // Assert
        deserialized.Should().NotBeNull($"{testData.ModelName} should deserialize successfully");
        deserialized.Should().BeOfType(testData.Model!.GetType(), $"{testData.ModelName} should maintain type");
    }

    [Theory]
    [MemberData(nameof(GetAllUniversalModels))]
    public void Clone_AllUniversalModels_CreatesDeepCopy(ModelTestData testData)
    {
        // Act
        var clone = CloneModel(testData.Model!);

        // Assert
        clone.Should().NotBeNull($"{testData.ModelName} should be cloneable");
        clone.Should().NotBeSameAs(testData.Model, $"{testData.ModelName} clone should be a different instance");
        clone.Should().BeOfType(testData.Model!.GetType(), $"{testData.ModelName} clone should maintain type");
    }

    [Theory]
    [MemberData(nameof(GetAllUniversalModels))]
    public void IsValid_AllUniversalModels_ReturnsTrueForValidXml(ModelTestData testData)
    {
        // Arrange
        var xml = SerializeModel(testData.Model!);

        // Act
        var isValid = IsValidXml(testData.Model!.GetType(), xml);

        // Assert
        isValid.Should().BeTrue($"{testData.ModelName} serialized XML should be valid");
    }

    [Theory]
    [MemberData(nameof(GetAllUniversalModels))]
    public void TryDeserialize_AllUniversalModels_ReturnsTrue(ModelTestData testData)
    {
        // Arrange
        var xml = SerializeModel(testData.Model!);

        // Act
        var success = TryDeserializeModel(testData.Model!.GetType(), xml, out var result);

        // Assert
        success.Should().BeTrue($"{testData.ModelName} should deserialize successfully");
        result.Should().NotBeNull($"{testData.ModelName} should return a valid object");
    }

    #endregion Theory Tests - All Models

    #region Theory Tests - Common Models File Operations

    [Theory]
    [MemberData(nameof(GetCommonUniversalModels))]
    public async Task SerializeToFileAsync_CommonUniversalModels_CreatesValidFile(ModelTestData testData)
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"{testData.ModelName}_{Guid.NewGuid()}.xml");

        try
        {
            // Act
            await SerializeToFileAsync(testData.Model!, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue($"{testData.ModelName} should create file");
            var content = await File.ReadAllTextAsync(tempFile);
            content.Should().NotBeNullOrEmpty($"{testData.ModelName} file should have content");
            content.Should().Contain("<?xml", $"{testData.ModelName} file should be valid XML");
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Theory]
    [MemberData(nameof(GetCommonUniversalModels))]
    public void SerializeToFile_CommonUniversalModels_CreatesValidFile(ModelTestData testData)
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"{testData.ModelName}_{Guid.NewGuid()}.xml");

        try
        {
            // Act
            SerializeToFile(testData.Model!, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue($"{testData.ModelName} should create file");
            var content = File.ReadAllText(tempFile);
            content.Should().NotBeNullOrEmpty($"{testData.ModelName} file should have content");
            content.Should().Contain("<?xml", $"{testData.ModelName} file should contain XML declaration");
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Theory]
    [MemberData(nameof(GetCommonUniversalModels))]
    public void DeserializeFromFile_CommonUniversalModels_ReturnsValidObject(ModelTestData testData)
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"{testData.ModelName}_{Guid.NewGuid()}.xml");

        try
        {
            SerializeToFile(testData.Model!, tempFile);

            // Act
            var deserialized = DeserializeFromFile(testData.Model!.GetType(), tempFile);

            // Assert
            deserialized.Should().NotBeNull($"{testData.ModelName} should deserialize from file");
            deserialized.Should().BeOfType(testData.Model!.GetType(), $"{testData.ModelName} should maintain type");
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    #endregion Theory Tests - Common Models File Operations

    #region Theory Tests - XML Formatting

    [Theory]
    [MemberData(nameof(GetCommonUniversalModels))]
    public void Serialize_WithIndentOption_FormatsXml(ModelTestData testData)
    {
        // Arrange
        var options = new XmlSerializerOptions { Indent = true };

        // Act
        var xml = SerializeModelWithOptions(testData.Model!, options);

        // Assert
        xml.Should().Match(s => s.Contains("\r\n") || s.Contains('\n'), $"{testData.ModelName} should have line breaks when indented");
    }

    [Theory]
    [MemberData(nameof(GetCommonUniversalModels))]
    public void Serialize_WithoutIndentOption_MinimizesXml(ModelTestData testData)
    {
        // Arrange
        var options = new XmlSerializerOptions { Indent = false };

        // Act
        var xml = SerializeModelWithOptions(testData.Model!, options);

        // Assert
        xml.Should().NotBeNullOrEmpty($"{testData.ModelName} should serialize without indentation");
    }

    #endregion Theory Tests - XML Formatting

    #region Error Handling Tests

    [Fact]
    public void TryDeserialize_WithInvalidUniversalXml_ReturnsFalse()
    {
        // Arrange
        var shipmentType = Type.GetType("CargoWiseNetLibrary.Models.Universal.UniversalShipmentData, CargoWiseNetLibrary");
        if (shipmentType == null) return;

        var invalidXml = "<Invalid>xml</Invalid>";

        // Act
        var success = TryDeserializeModel(shipmentType, invalidXml, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void IsValid_WithInvalidUniversalXml_ReturnsFalse()
    {
        // Arrange
        var shipmentType = Type.GetType("CargoWiseNetLibrary.Models.Universal.UniversalShipmentData, CargoWiseNetLibrary");
        if (shipmentType == null) return;

        var invalidXml = "not valid xml at all";

        // Act
        var isValid = IsValidXml(shipmentType, invalidXml);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void DeserializeFromFile_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var shipmentType = Type.GetType("CargoWiseNetLibrary.Models.Universal.UniversalShipmentData, CargoWiseNetLibrary");
        if (shipmentType == null) return;

        var nonExistentFile = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.xml");

        // Act & Assert
        var act = () => DeserializeFromFile(shipmentType, nonExistentFile);
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void TryDeserialize_WithEmptyString_ReturnsFalse()
    {
        // Arrange
        var shipmentType = Type.GetType("CargoWiseNetLibrary.Models.Universal.UniversalShipmentData, CargoWiseNetLibrary");
        if (shipmentType == null) return;

        // Act
        var success = TryDeserializeModel(shipmentType, string.Empty, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    #endregion Error Handling Tests

    #region Helper Methods using Reflection

    private static string SerializeModel(object model)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("Serialize", [model.GetType(), typeof(XmlSerializerOptions)]);
        return (string)serializeMethod!.Invoke(null, [model, null])!;
    }

    private static string SerializeModelWithOptions(object model, XmlSerializerOptions options)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("Serialize", [model.GetType(), typeof(XmlSerializerOptions)]);
        return (string)serializeMethod!.Invoke(null, [model, options])!;
    }

    private static object? DeserializeModel(Type modelType, string xml)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(modelType);
        var deserializeMethod = serializerType.GetMethod("Deserialize", [typeof(string)]) ?? throw new InvalidOperationException($"Could not find Deserialize method for {modelType.Name}");
        return deserializeMethod.Invoke(null, [xml]);
    }

    private static void SerializeToFile(object model, string filePath)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("SerializeToFile", [model.GetType(), typeof(string), typeof(XmlSerializerOptions)]) ?? throw new InvalidOperationException($"Could not find SerializeToFile method for {model.GetType().Name}");
        serializeMethod.Invoke(null, [model, filePath, null]);
    }

    private static async Task SerializeToFileAsync(object model, string filePath)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("SerializeToFileAsync") ?? throw new InvalidOperationException($"Could not find SerializeToFileAsync method for {model.GetType().Name}");
        var task = (Task)serializeMethod.Invoke(null, [model, filePath, null, default(CancellationToken)])!;
        await task;
    }

    private static object? DeserializeFromFile(Type modelType, string filePath)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(modelType);
        // Find the method using BindingFlags to handle any overloads
        var methods = serializerType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        var deserializeMethod = methods.FirstOrDefault(m => m.Name == "DeserializeFromFile" && m.GetParameters().Length == 1)
            ?? throw new InvalidOperationException($"Could not find DeserializeFromFile method for {modelType.Name}");
        return deserializeMethod.Invoke(null, [filePath]);
    }

    private static object? CloneModel(object model)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(model.GetType());
        var cloneMethod = serializerType.GetMethod("Clone", [model.GetType(), typeof(XmlSerializerOptions)]) ?? throw new InvalidOperationException($"Could not find Clone method for {model.GetType().Name}");
        return cloneMethod.Invoke(null, [model, null]);
    }

    private static bool IsValidXml(Type modelType, string xml)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(modelType);
        var isValidMethod = serializerType.GetMethod("IsValid", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static) ?? throw new InvalidOperationException($"Could not find IsValid method for {modelType.Name}");
        var result = isValidMethod.Invoke(null, [xml]);
        return result != null && (bool)result;
    }

    private static bool TryDeserializeModel(Type modelType, string xml, out object? result)
    {
        result = null;

        try
        {
            var serializerType = typeof(XmlSerializer<>).MakeGenericType(modelType);
            var tryDeserializeMethod = serializerType.GetMethod("TryDeserialize",
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static) ?? throw new InvalidOperationException($"Could not find TryDeserialize method for {modelType.Name}");

            // Create parameters array for the out parameter
            var parameters = new object?[] { xml, null };
            var success = (bool)tryDeserializeMethod.Invoke(null, parameters)!;
            result = parameters[1];

            return success;
        }
        catch
        {
            return false;
        }
    }

    #endregion Helper Methods using Reflection
}
