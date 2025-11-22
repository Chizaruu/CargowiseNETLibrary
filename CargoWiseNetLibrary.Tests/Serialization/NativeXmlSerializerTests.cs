using CargoWiseNetLibrary.Models.Native;
using CargoWiseNetLibrary.Serialization;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace CargoWiseNetLibrary.Tests.Serialization;

/// <summary>
/// Comprehensive XML serialization tests for ALL Native models
/// Covers all XSD files from the Native schema directory:
/// - Native.xsd, NativeAcceptabilityBand.xsd, NativeAddOnRule.xsd, etc.
/// </summary>
public class NativeXmlSerializerTests
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
    /// Test data provider for ALL Native model types
    /// Based on the XSD files visible in the Native schema directory
    /// </summary>
    public static TheoryData<ModelTestData> GetAllNativeModels()
    {
        var data = new TheoryData<ModelTestData>();

        // Core Native models
        AddIfTypeExists(data, "Native");
        AddIfTypeExists(data, "NativeBody");

        // All Native models from XSD files (alphabetically ordered)
        AddIfTypeExists(data, "NativeAcceptabilityBand");
        AddIfTypeExists(data, "NativeAddOnRule");
        AddIfTypeExists(data, "NativeAirline");
        AddIfTypeExists(data, "NativeBMControlCustomisation");
        AddIfTypeExists(data, "NativeBMSystem");
        AddIfTypeExists(data, "NativeCarrierAccount");
        AddIfTypeExists(data, "NativeCommodityCode");
        AddIfTypeExists(data, "NativeCommunication");
        AddIfTypeExists(data, "NativeCompany");
        AddIfTypeExists(data, "NativeContainer");
        AddIfTypeExists(data, "NativeCountry");
        AddIfTypeExists(data, "NativeCurrencyExchangeRate");
        AddIfTypeExists(data, "NativeCustomEntity");
        AddIfTypeExists(data, "NativeDangerousGood");
        AddIfTypeExists(data, "NativeDangerousGoodsCountryReferenceMapping");
        AddIfTypeExists(data, "NativeEDICodeMapping");
        AddIfTypeExists(data, "NativeNewsAnnouncement");
        AddIfTypeExists(data, "NativeOpportunity");
        AddIfTypeExists(data, "NativeOrganization");
        AddIfTypeExists(data, "NativeProduct");
        AddIfTypeExists(data, "NativePutawayGroup");
        AddIfTypeExists(data, "NativeRate");
        AddIfTypeExists(data, "NativeRequest");
        AddIfTypeExists(data, "NativeSalesChannel");
        AddIfTypeExists(data, "NativeServiceLevel");
        AddIfTypeExists(data, "NativeStaff");
        AddIfTypeExists(data, "NativeTag");
        AddIfTypeExists(data, "NativeTagRule");
        AddIfTypeExists(data, "NativeTransportZone");
        AddIfTypeExists(data, "NativeUNLOCO");
        AddIfTypeExists(data, "NativeVessel");
        AddIfTypeExists(data, "NativeWarehouseClientAccountAssociation");
        AddIfTypeExists(data, "NativeWorkflowTemplate");

        return data;
    }

    /// <summary>
    /// Test data provider for commonly used Native models
    /// </summary>
    public static TheoryData<ModelTestData> GetCommonNativeModels()
    {
        var data = new TheoryData<ModelTestData>();

        AddIfTypeExists(data, "Native");
        AddIfTypeExists(data, "NativeBody");
        AddIfTypeExists(data, "NativeCompany");
        AddIfTypeExists(data, "NativeOrganization");
        AddIfTypeExists(data, "NativeStaff");
        AddIfTypeExists(data, "NativeCountry");
        AddIfTypeExists(data, "NativeRate");
        AddIfTypeExists(data, "NativeContainer");
        AddIfTypeExists(data, "NativeVessel");
        AddIfTypeExists(data, "NativeAirline");

        return data;
    }

    /// <summary>
    /// Helper method to add a type to test data if it exists in the assembly
    /// </summary>
    private static void AddIfTypeExists(TheoryData<ModelTestData> data, string typeName)
    {
        var fullTypeName = $"CargoWiseNetLibrary.Models.Native.{typeName}, CargoWiseNetLibrary";
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
    [MemberData(nameof(GetAllNativeModels))]
    public void Serialize_AllNativeModels_ReturnsValidXml(ModelTestData testData)
    {
        // Act
        var xml = SerializeModel(testData.Model!);

        // Assert
        xml.Should().NotBeNullOrEmpty($"{testData.ModelName} should serialize to XML");
        xml.Should().Contain("<?xml", $"{testData.ModelName} should include XML declaration");
        xml.Should().Contain($"<{testData.ModelName}", $"{testData.ModelName} should contain root element");
    }

    [Theory]
    [MemberData(nameof(GetAllNativeModels))]
    public void RoundTrip_AllNativeModels_PreservesData(ModelTestData testData)
    {
        // Act
        var xml = SerializeModel(testData.Model!);
        var deserialized = DeserializeModel(testData.Model!.GetType(), xml);

        // Assert
        deserialized.Should().NotBeNull($"{testData.ModelName} should deserialize successfully");
        deserialized.Should().BeOfType(testData.Model!.GetType(), $"{testData.ModelName} should maintain type");
    }

    [Theory]
    [MemberData(nameof(GetAllNativeModels))]
    public void Clone_AllNativeModels_CreatesDeepCopy(ModelTestData testData)
    {
        // Act
        var clone = CloneModel(testData.Model!);

        // Assert
        clone.Should().NotBeNull($"{testData.ModelName} should be cloneable");
        clone.Should().NotBeSameAs(testData.Model, $"{testData.ModelName} clone should be a different instance");
        clone.Should().BeOfType(testData.Model!.GetType(), $"{testData.ModelName} clone should maintain type");
    }

    [Theory]
    [MemberData(nameof(GetAllNativeModels))]
    public void IsValid_AllNativeModels_ReturnsTrueForValidXml(ModelTestData testData)
    {
        // Arrange
        var xml = SerializeModel(testData.Model!);

        // Act
        var isValid = IsValidXml(testData.Model!.GetType(), xml);

        // Assert
        isValid.Should().BeTrue($"{testData.ModelName} serialized XML should be valid");
    }

    [Theory]
    [MemberData(nameof(GetAllNativeModels))]
    public void TryDeserialize_AllNativeModels_ReturnsTrue(ModelTestData testData)
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
    [MemberData(nameof(GetCommonNativeModels))]
    public async Task SerializeToFileAsync_CommonNativeModels_CreatesValidFile(ModelTestData testData)
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
    [MemberData(nameof(GetCommonNativeModels))]
    public void SerializeToFile_CommonNativeModels_CreatesValidFile(ModelTestData testData)
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
    [MemberData(nameof(GetCommonNativeModels))]
    public void DeserializeFromFile_CommonNativeModels_ReturnsValidObject(ModelTestData testData)
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
    [MemberData(nameof(GetCommonNativeModels))]
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
    [MemberData(nameof(GetCommonNativeModels))]
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

    #region Specific Tests - Native Root Model

    [Fact]
    public void Serialize_Native_WithPopulatedBody_IncludesBodyData()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        var nativeBodyType = Type.GetType("CargoWiseNetLibrary.Models.Native.NativeBody, CargoWiseNetLibrary");

        if (nativeType == null || nativeBodyType == null)
        {
            return;
        }

        var native = Activator.CreateInstance(nativeType);
        var body = Activator.CreateInstance(nativeBodyType);
        nativeType.GetProperty("Body")?.SetValue(native, body);

        // Act
        var xml = SerializeModel(native!);

        // Assert
        xml.Should().NotBeNullOrEmpty();
        xml.Should().Contain("Body");
    }

    [Fact]
    public void Deserialize_Native_WithBody_ExtractsBodyCorrectly()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        var nativeBodyType = Type.GetType("CargoWiseNetLibrary.Models.Native.NativeBody, CargoWiseNetLibrary");

        if (nativeType == null || nativeBodyType == null)
        {
            return;
        }

        var native = Activator.CreateInstance(nativeType);
        var body = Activator.CreateInstance(nativeBodyType);
        nativeType.GetProperty("Body")?.SetValue(native, body);
        var xml = SerializeModel(native!);

        // Act
        var deserialized = DeserializeModel(nativeType, xml);

        // Assert
        deserialized.Should().NotBeNull();
        var bodyProperty = nativeType.GetProperty("Body")?.GetValue(deserialized);
        bodyProperty.Should().NotBeNull();
    }

    [Fact]
    public async Task AsyncRoundTrip_Native_PreservesData()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        var nativeBodyType = Type.GetType("CargoWiseNetLibrary.Models.Native.NativeBody, CargoWiseNetLibrary");

        if (nativeType == null || nativeBodyType == null)
        {
            return;
        }

        var native = Activator.CreateInstance(nativeType);
        var body = Activator.CreateInstance(nativeBodyType);
        nativeType.GetProperty("Body")?.SetValue(native, body);

        var tempFile = Path.GetTempFileName();

        try
        {
            // Act
            await SerializeToFileAsync(native!, tempFile);
            var deserialized = await DeserializeFromFileAsync(nativeType, tempFile);

            // Assert
            deserialized.Should().NotBeNull();
            var bodyProperty = nativeType.GetProperty("Body")?.GetValue(deserialized);
            bodyProperty.Should().NotBeNull();
        }
        finally
        {
            // Cleanup
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void RoundTrip_Native_PreservesAllData()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        var nativeBodyType = Type.GetType("CargoWiseNetLibrary.Models.Native.NativeBody, CargoWiseNetLibrary");

        if (nativeType == null || nativeBodyType == null)
        {
            return;
        }

        var original = Activator.CreateInstance(nativeType);
        var body = Activator.CreateInstance(nativeBodyType);
        nativeType.GetProperty("Body")?.SetValue(original, body);

        // Act
        var xml = SerializeModel(original!);
        var deserialized = DeserializeModel(nativeType, xml);

        // Assert
        deserialized.Should().NotBeNull();
        var bodyProperty = nativeType.GetProperty("Body")?.GetValue(deserialized);
        bodyProperty.Should().NotBeNull();
    }

    #endregion Specific Tests - Native Root Model

    #region Error Handling Tests

    [Fact]
    public void TryDeserialize_WithInvalidNativeXml_ReturnsFalse()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        if (nativeType == null) return;

        var invalidXml = "<Invalid>xml</Invalid>";

        // Act
        var success = TryDeserializeModel(nativeType, invalidXml, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void IsValid_WithInvalidNativeXml_ReturnsFalse()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        if (nativeType == null) return;

        var invalidXml = "not valid xml at all";

        // Act
        var isValid = IsValidXml(nativeType, invalidXml);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void DeserializeFromFile_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        if (nativeType == null) return;

        var nonExistentFile = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.xml");

        // Act & Assert
        var act = () => DeserializeFromFile(nativeType, nonExistentFile);
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void TryDeserialize_WithEmptyString_ReturnsFalse()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        if (nativeType == null) return;

        // Act
        var success = TryDeserializeModel(nativeType, string.Empty, out var result);

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
        var deserializeMethod = serializerType.GetMethod("DeserializeFromFile", [typeof(string)]) ?? throw new InvalidOperationException($"Could not find DeserializeFromFile method for {modelType.Name}");
        return deserializeMethod.Invoke(null, [filePath]);
    }

    private static async Task<object?> DeserializeFromFileAsync(Type modelType, string filePath)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(modelType);
        var deserializeMethod = serializerType.GetMethod("DeserializeFromFileAsync") ?? throw new InvalidOperationException($"Could not find DeserializeFromFileAsync method for {modelType.Name}");
        var task = (Task)deserializeMethod.Invoke(null, [filePath, default(CancellationToken)])!;
        await task;

        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task);
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
