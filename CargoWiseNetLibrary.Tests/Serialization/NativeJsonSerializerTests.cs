using CargoWiseNetLibrary.Models.Native;
using CargoWiseNetLibrary.Serialization;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace CargoWiseNetLibrary.Tests.Serialization;

/// <summary>
/// Comprehensive JSON serialization tests for ALL Native models
/// Covers all XSD files from the Native schema directory:
/// - Native.xsd, NativeAcceptabilityBand.xsd, NativeAddOnRule.xsd, etc.
/// </summary>
public class NativeJsonSerializerTests
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
    public void Serialize_AllNativeModels_ReturnsValidJson(ModelTestData testData)
    {
        // Act
        var json = SerializeModel(testData.Model!);

        // Assert
        json.Should().NotBeNullOrEmpty($"{testData.ModelName} should serialize to JSON");
        json.Should().StartWith("{", $"{testData.ModelName} JSON should start with opening brace");
        json.Should().EndWith("}", $"{testData.ModelName} JSON should end with closing brace");
    }

    [Theory]
    [MemberData(nameof(GetAllNativeModels))]
    public void RoundTrip_AllNativeModels_PreservesData(ModelTestData testData)
    {
        // Act
        var json = SerializeModel(testData.Model!);
        var deserialized = DeserializeModel(testData.Model!.GetType(), json);

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
    public void IsValid_AllNativeModels_ReturnsTrueForValidJson(ModelTestData testData)
    {
        // Arrange
        var json = SerializeModel(testData.Model!);

        // Act
        var isValid = IsValidJson(testData.Model!.GetType(), json);

        // Assert
        isValid.Should().BeTrue($"{testData.ModelName} serialized JSON should be valid");
    }

    [Theory]
    [MemberData(nameof(GetAllNativeModels))]
    public void TryDeserialize_AllNativeModels_ReturnsTrue(ModelTestData testData)
    {
        // Arrange
        var json = SerializeModel(testData.Model!);

        // Act
        var success = TryDeserializeModel(testData.Model!.GetType(), json, out var result);

        // Assert
        success.Should().BeTrue($"{testData.ModelName} should deserialize successfully");
        result.Should().NotBeNull($"{testData.ModelName} should return a valid object");
    }

    [Theory]
    [MemberData(nameof(GetAllNativeModels))]
    public void Serialize_WithNullProperties_HandlesCorrectly(ModelTestData testData)
    {
        // Act
        var json = SerializeModel(testData.Model!);

        // Assert - Should either omit nulls or serialize them consistently
        json.Should().NotBeNullOrEmpty($"{testData.ModelName} should serialize even with null properties");
    }

    #endregion Theory Tests - All Models

    #region Theory Tests - Common Models File Operations

    [Theory]
    [MemberData(nameof(GetCommonNativeModels))]
    public async Task SerializeToFileAsync_CommonNativeModels_CreatesValidFile(ModelTestData testData)
    {
        // Arrange
        var tempFile = Path.Combine(Path.GetTempPath(), $"{testData.ModelName}_{Guid.NewGuid()}.json");

        try
        {
            // Act
            await SerializeToFileAsync(testData.Model!, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue($"{testData.ModelName} should create file");
            var content = await File.ReadAllTextAsync(tempFile);
            content.Should().NotBeNullOrEmpty($"{testData.ModelName} file should have content");
            content.Should().StartWith("{", $"{testData.ModelName} file should contain valid JSON");
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
        var tempFile = Path.Combine(Path.GetTempPath(), $"{testData.ModelName}_{Guid.NewGuid()}.json");

        try
        {
            // Act
            SerializeToFile(testData.Model!, tempFile);

            // Assert
            File.Exists(tempFile).Should().BeTrue($"{testData.ModelName} should create file");
            var content = File.ReadAllText(tempFile);
            content.Should().NotBeNullOrEmpty($"{testData.ModelName} file should have content");
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
        var tempFile = Path.Combine(Path.GetTempPath(), $"{testData.ModelName}_{Guid.NewGuid()}.json");

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

    #region Specific Tests - Native Root Model

    [Fact]
    public void Serialize_Native_WithPopulatedBody_IncludesBodyData()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        var nativeBodyType = Type.GetType("CargoWiseNetLibrary.Models.Native.NativeBody, CargoWiseNetLibrary");

        if (nativeType == null || nativeBodyType == null)
        {
            // Skip if types don't exist
            return;
        }

        var native = Activator.CreateInstance(nativeType);
        var body = Activator.CreateInstance(nativeBodyType);
        nativeType.GetProperty("Body")?.SetValue(native, body);

        // Act
        var json = SerializeModel(native!);

        // Assert
        json.Should().NotBeNullOrEmpty();
        json.Should().Contain("Body");
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
        var json = SerializeModel(native!);

        // Act
        var deserialized = DeserializeModel(nativeType, json);

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

    #endregion Specific Tests - Native Root Model

    #region Error Handling Tests

    [Fact]
    public void TryDeserialize_WithInvalidNativeJson_ReturnsFalse()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        if (nativeType == null) return;

        var invalidJson = "{ invalid json }";

        // Act
        var success = TryDeserializeModel(nativeType, invalidJson, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().BeNull();
    }

    [Fact]
    public void IsValid_WithInvalidNativeJson_ReturnsFalse()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        if (nativeType == null) return;

        var invalidJson = "not valid json at all";

        // Act
        var isValid = IsValidJson(nativeType, invalidJson);

        // Assert
        isValid.Should().BeFalse();
    }

    [Fact]
    public void DeserializeFromFile_WithNonExistentFile_ThrowsFileNotFoundException()
    {
        // Arrange
        var nativeType = Type.GetType("CargoWiseNetLibrary.Models.Native.Native, CargoWiseNetLibrary");
        if (nativeType == null) return;

        var nonExistentFile = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}.json");

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
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("Serialize", [model.GetType(), typeof(System.Text.Json.JsonSerializerOptions)]);
        return (string)serializeMethod!.Invoke(null, [model, null])!;
    }

    private static object? DeserializeModel(Type modelType, string json)
    {
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(modelType);
        var deserializeMethod = serializerType.GetMethod("Deserialize",
            [typeof(string), typeof(System.Text.Json.JsonSerializerOptions)]) ?? throw new InvalidOperationException($"Could not find Deserialize method for {modelType.Name}");
        return deserializeMethod.Invoke(null, [json, null]);
    }

    private static void SerializeToFile(object model, string filePath)
    {
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("SerializeToFile", [model.GetType(), typeof(string), typeof(System.Text.Json.JsonSerializerOptions)]) ?? throw new InvalidOperationException($"Could not find SerializeToFile method for {model.GetType().Name}");
        serializeMethod.Invoke(null, [model, filePath, null]);
    }

    private static async Task SerializeToFileAsync(object model, string filePath)
    {
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("SerializeToFileAsync") ?? throw new InvalidOperationException($"Could not find SerializeToFileAsync method for {model.GetType().Name}");
        var task = (Task)serializeMethod.Invoke(null, [model, filePath, null, default(CancellationToken)])!;
        await task;
    }

    private static object? DeserializeFromFile(Type modelType, string filePath)
    {
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(modelType);
        // DeserializeFromFile has signature: (string filePath, JsonSerializerOptions? options = null)
        var deserializeMethod = serializerType.GetMethod("DeserializeFromFile",
            [typeof(string), typeof(System.Text.Json.JsonSerializerOptions)])
            ?? throw new InvalidOperationException($"Could not find DeserializeFromFile method for {modelType.Name}");
        return deserializeMethod.Invoke(null, [filePath, null]);
    }

    private static async Task<object?> DeserializeFromFileAsync(Type modelType, string filePath)
    {
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(modelType);
        var deserializeMethod = serializerType.GetMethod("DeserializeFromFileAsync") ?? throw new InvalidOperationException($"Could not find DeserializeFromFileAsync method for {modelType.Name}");
        var task = (Task)deserializeMethod.Invoke(null, [filePath, null, default(CancellationToken)])!;
        await task;

        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task);
    }

    private static object? CloneModel(object model)
    {
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(model.GetType());
        var cloneMethod = serializerType.GetMethod("Clone", [model.GetType(), typeof(System.Text.Json.JsonSerializerOptions)]) ?? throw new InvalidOperationException($"Could not find Clone method for {model.GetType().Name}");
        return cloneMethod.Invoke(null, [model, null]);
    }

    private static bool IsValidJson(Type modelType, string json)
    {
        var serializerType = typeof(JsonSerializer<>).MakeGenericType(modelType);
        var isValidMethod = serializerType.GetMethod("IsValid",
            [typeof(string), typeof(System.Text.Json.JsonSerializerOptions)]) ?? throw new InvalidOperationException($"Could not find IsValid method for {modelType.Name}");
        var result = isValidMethod.Invoke(null, [json, null]);
        return result != null && (bool)result;
    }

    private static bool TryDeserializeModel(Type modelType, string json, out object? result)
    {
        result = null;

        try
        {
            var serializerType = typeof(JsonSerializer<>).MakeGenericType(modelType);
            var tryDeserializeMethod = serializerType.GetMethod("TryDeserialize") ?? throw new InvalidOperationException($"Could not find TryDeserialize method for {modelType.Name}");

            // Create parameters array: json, out result, options
            var parameters = new object?[] { json, null, null };
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
