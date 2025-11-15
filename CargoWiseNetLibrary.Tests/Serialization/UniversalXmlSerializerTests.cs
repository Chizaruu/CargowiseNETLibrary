using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace CargoWiseNetLibrary.Tests.Serialization;

/// <summary>
/// Tests XML serialization for all Universal models
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

        // Required for deserialization
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

                // Initialize specific properties based on type
                if (Model is UniversalShipmentData shipmentData)
                {
                    shipmentData.version = "1.1";
                    shipmentData.Shipment = new Shipment();
                }
                else if (Model is UniversalScheduleData scheduleData)
                {
                    scheduleData.version = "1.1";
                    scheduleData.Schedule = new Schedule();
                }
                else if (Model is UniversalTransactionData transactionData)
                {
                    transactionData.version = "1.1";
                    transactionData.TransactionInfo = new TransactionInfo();
                }
                else if (Model is UniversalTransactionBatchData transactionBatchData)
                {
                    transactionBatchData.version = "1.1";
                    transactionBatchData.TransactionBatch = new TransactionBatch();
                }
                else if (Model is UniversalShipmentRequestData shipmentRequestData)
                {
                    shipmentRequestData.version = "1.1";
                    shipmentRequestData.ShipmentRequest = new ShipmentRequest();
                }
                else if (Model is UniversalTransactionBatchRequestData transactionBatchRequestData)
                {
                    transactionBatchRequestData.version = "1.1";
                    transactionBatchRequestData.TransactionBatchRequest = new TransactionBatchRequest();
                }
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

    // Test data provider for main Universal root models
    public static TheoryData<ModelTestData> GetUniversalRootModels()
    {
        return
        [
            new(
                new UniversalShipmentData
                {
                    version = "1.1",
                    Shipment = new Shipment()
                },
                "UniversalShipmentData"
            ),
            new(
                new UniversalScheduleData
                {
                    version = "1.1",
                    Schedule = new Schedule()
                },
                "UniversalScheduleData"
            ),
            new(
                new UniversalTransactionData
                {
                    version = "1.1",
                    TransactionInfo = new TransactionInfo()
                },
                "UniversalTransactionData"
            ),
            new(
                new UniversalTransactionBatchData
                {
                    version = "1.1",
                    TransactionBatch = new TransactionBatch()
                },
                "UniversalTransactionBatchData"
            ),
            new(
                new UniversalShipmentRequestData
                {
                    version = "1.1",
                    ShipmentRequest = new ShipmentRequest()
                },
                "UniversalShipmentRequestData"
            ),
            new(
                new UniversalTransactionBatchRequestData
                {
                    version = "1.1",
                    TransactionBatchRequest = new TransactionBatchRequest()
                },
                "UniversalTransactionBatchRequestData"
            )
        ];
    }

    // Test data provider for common nested models
    public static TheoryData<ModelTestData> GetCommonModels()
    {
        return
        [
            new(new Activity(), "Activity"),
            new(new Branch(), "Branch"),
            new(new Company(), "Company"),
            new(new Staff(), "Staff"),
            new(new Department(), "Department"),
            new(new CodeDescriptionPair(), "CodeDescriptionPair"),
            new(new DataContext(), "DataContext"),
            new(new CustomizedField(), "CustomizedField"),
            new(new OrganizationAddress(), "OrganizationAddress"),
            new(new Milestone(), "Milestone")
        ];
    }

    [Theory]
    [MemberData(nameof(GetUniversalRootModels))]
    public void Serialize_UniversalRootModels_ReturnsValidXml(ModelTestData testData)
    {
        // Act
        var xml = SerializeModel(testData.Model!);

        // Assert
        xml.Should().NotBeNullOrEmpty($"{testData.ModelName} should serialize to XML");
        xml.Should().Contain("<?xml", $"{testData.ModelName} should include XML declaration");
        xml.Should().Contain($"<{GetRootElementName(testData.ModelName)}", $"{testData.ModelName} should contain root element");
    }

    [Theory]
    [MemberData(nameof(GetUniversalRootModels))]
    public void RoundTrip_UniversalRootModels_PreservesData(ModelTestData testData)
    {
        // Act
        var xml = SerializeModel(testData.Model!);
        var deserialized = DeserializeModel(testData.Model!.GetType(), xml);

        // Assert
        deserialized.Should().NotBeNull($"{testData.ModelName} should deserialize successfully");
        deserialized.Should().BeOfType(testData.Model!.GetType(), $"{testData.ModelName} should maintain type");
    }

    [Theory]
    [MemberData(nameof(GetUniversalRootModels))]
    public async Task SerializeToFileAsync_UniversalRootModels_CreatesValidFile(ModelTestData testData)
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
    [MemberData(nameof(GetCommonModels))]
    public void Serialize_CommonModels_ReturnsValidXml(ModelTestData testData)
    {
        // Act
        var xml = SerializeModel(testData.Model!);

        // Assert
        xml.Should().NotBeNullOrEmpty($"{testData.ModelName} should serialize to XML");
        xml.Should().Contain("<?xml", $"{testData.ModelName} should include XML declaration");
    }

    [Theory]
    [MemberData(nameof(GetCommonModels))]
    public void Clone_CommonModels_CreatesDeepCopy(ModelTestData testData)
    {
        // Act
        var clone = CloneModel(testData.Model!);

        // Assert
        clone.Should().NotBeNull($"{testData.ModelName} should be cloneable");
        clone.Should().NotBeSameAs(testData.Model, $"{testData.ModelName} clone should be a different instance");
        clone.Should().BeOfType(testData.Model!.GetType(), $"{testData.ModelName} clone should maintain type");
    }

    [Theory]
    [MemberData(nameof(GetCommonModels))]
    public void IsValid_CommonModels_ReturnsTrueForValidXml(ModelTestData testData)
    {
        // Arrange
        var xml = SerializeModel(testData.Model!);

        // Act
        var isValid = IsValidXml(testData.Model!.GetType(), xml);

        // Assert
        isValid.Should().BeTrue($"{testData.ModelName} serialized XML should be valid");
    }

    [Theory]
    [MemberData(nameof(GetCommonModels))]
    public void TryDeserialize_CommonModels_ReturnsTrue(ModelTestData testData)
    {
        // Arrange
        var xml = SerializeModel(testData.Model!);

        // Act
        var success = TryDeserializeModel(testData.Model!.GetType(), xml, out var result);

        // Assert
        success.Should().BeTrue($"{testData.ModelName} should deserialize successfully");
        result.Should().NotBeNull($"{testData.ModelName} should return a valid object");
    }

    #region Helper Methods using Reflection

    private static string SerializeModel(object model)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("Serialize", [model.GetType(), typeof(XmlSerializerOptions)]);
        return (string)serializeMethod!.Invoke(null, [model, null])!;
    }

    private static object? DeserializeModel(Type modelType, string xml)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(modelType);
        var deserializeMethod = serializerType.GetMethod("Deserialize", [typeof(string)]) ?? throw new InvalidOperationException($"Could not find Deserialize method for {modelType.Name}");
        return deserializeMethod.Invoke(null, [xml]);
    }

    private static async Task SerializeToFileAsync(object model, string filePath)
    {
        var serializerType = typeof(XmlSerializer<>).MakeGenericType(model.GetType());
        var serializeMethod = serializerType.GetMethod("SerializeToFileAsync") ?? throw new InvalidOperationException($"Could not find SerializeToFileAsync method for {model.GetType().Name}");
        var task = (Task)serializeMethod.Invoke(null, [model, filePath, null, default(CancellationToken)])!;
        await task;
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

    private static string GetRootElementName(string modelName)
    {
        // Map model names to their XML root element names
        return modelName switch
        {
            "UniversalShipmentData" => "UniversalShipment",
            "UniversalScheduleData" => "UniversalSchedule",
            "UniversalTransactionData" => "UniversalTransaction",
            "UniversalTransactionBatchData" => "UniversalTransactionBatch",
            "UniversalShipmentRequestData" => "UniversalShipmentRequest",
            "UniversalTransactionBatchRequestData" => "UniversalTransactionBatchRequest",
            _ => modelName
        };
    }

    #endregion Helper Methods using Reflection
}
