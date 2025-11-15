using CargoWiseNetLibrary.Models.Universal;
using CargoWiseNetLibrary.Serialization;
using FluentAssertions;
using Xunit;

namespace CargoWiseNetLibrary.Tests.Serialization;

/// <summary>
/// Tests for UniversalInterchange deserialization and body extraction
/// </summary>
public class UniversalInterchangeTests
{
    [Fact]
    public void Deserialize_UniversalInterchange_WithShipment_ExtractsShipmentData()
    {
        // Arrange
        var expectedShipment = new Shipment();
        var shipmentData = new UniversalShipmentData
        {
            version = "1.1",
            Shipment = expectedShipment
        };

        var interchange = UniversalInterchangeHelper.CreateInterchange(shipmentData);
        var xml = XmlSerializer<UniversalInterchange>.Serialize(interchange);

        // Act
        var deserialized = XmlSerializer<UniversalInterchange>.Deserialize(xml);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Body.Should().NotBeNull();
        deserialized.HasData().Should().BeTrue();

        var extractedShipment = deserialized.ExtractData<UniversalShipmentData>();
        extractedShipment.Should().NotBeNull();
        extractedShipment!.version.Should().Be("1.1");
        extractedShipment.Shipment.Should().NotBeNull();
    }

    [Fact]
    public void Deserialize_UniversalInterchange_WithSchedule_ExtractsScheduleData()
    {
        // Arrange
        var scheduleData = new UniversalScheduleData
        {
            version = "1.1",
            Schedule = new Schedule()
        };

        var interchange = UniversalInterchangeHelper.CreateInterchange(scheduleData);
        var xml = XmlSerializer<UniversalInterchange>.Serialize(interchange);

        // Act
        var deserialized = XmlSerializer<UniversalInterchange>.Deserialize(xml);

        // Assert
        deserialized.Should().NotBeNull();

        var extractedSchedule = deserialized!.ExtractData<UniversalScheduleData>();
        extractedSchedule.Should().NotBeNull();
        extractedSchedule!.version.Should().Be("1.1");
        extractedSchedule.Schedule.Should().NotBeNull();
    }

    [Fact]
    public void Deserialize_UniversalInterchange_WithTransaction_ExtractsTransactionData()
    {
        // Arrange
        var transactionData = new UniversalTransactionData
        {
            version = "1.1",
            TransactionInfo = new TransactionInfo()
        };

        var interchange = UniversalInterchangeHelper.CreateInterchange(transactionData);
        var xml = XmlSerializer<UniversalInterchange>.Serialize(interchange);

        // Act
        var deserialized = XmlSerializer<UniversalInterchange>.Deserialize(xml);

        // Assert
        deserialized.Should().NotBeNull();

        var extractedTransaction = deserialized!.ExtractData<UniversalTransactionData>();
        extractedTransaction.Should().NotBeNull();
        extractedTransaction!.version.Should().Be("1.1");
        extractedTransaction.TransactionInfo.Should().NotBeNull();
    }

    [Theory]
    [InlineData(typeof(UniversalShipmentData))]
    [InlineData(typeof(UniversalScheduleData))]
    [InlineData(typeof(UniversalTransactionData))]
    [InlineData(typeof(UniversalTransactionBatchData))]
    [InlineData(typeof(UniversalShipmentRequestData))]
    [InlineData(typeof(UniversalTransactionBatchRequestData))]
    public void RoundTrip_UniversalInterchange_PreservesDataType(Type dataType)
    {
        // Arrange
        var data = CreateUniversalData(dataType);
        var interchange = UniversalInterchangeHelper.CreateInterchange(data);

        // Act
        var xml = XmlSerializer<UniversalInterchange>.Serialize(interchange);
        var deserialized = XmlSerializer<UniversalInterchange>.Deserialize(xml);

        // Assert
        deserialized.Should().NotBeNull();
        deserialized!.Body.Should().NotBeNull();
        deserialized.HasData().Should().BeTrue();

        var extracted = ExtractDataByType(deserialized, dataType);
        extracted.Should().NotBeNull();
        extracted.Should().BeOfType(dataType);
    }

    [Fact]
    public void GetDataType_UniversalInterchange_WithShipment_ReturnsCorrectType()
    {
        // Arrange
        var interchange = UniversalInterchangeHelper.CreateInterchange(new UniversalShipmentData
        {
            version = "1.1",
            Shipment = new Shipment()
        });

        // Act
        var dataType = interchange.GetDataType();

        // Assert
        dataType.Should().Be(typeof(UniversalShipmentData));
    }

    [Fact]
    public void TryExtractShipment_WithShipmentData_ReturnsTrue()
    {
        // Arrange
        var interchange = UniversalInterchangeHelper.CreateInterchange(new UniversalShipmentData
        {
            version = "1.1",
            Shipment = new Shipment()
        });

        // Act
        var success = interchange.TryExtractData<UniversalShipmentData>(out var shipmentData);

        // Assert
        success.Should().BeTrue();
        shipmentData.Should().NotBeNull();
        shipmentData!.Shipment.Should().NotBeNull();
    }

    [Fact]
    public void TryExtractShipment_WithScheduleData_ReturnsFalse()
    {
        // Arrange
        var interchange = UniversalInterchangeHelper.CreateInterchange(new UniversalScheduleData
        {
            version = "1.1",
            Schedule = new Schedule()
        });

        // Act - Try to extract as Shipment (wrong type)
        var success = interchange.TryExtractData<UniversalShipmentData>(out var shipmentData);

        // Assert - Should fail because it's actually a Schedule
        success.Should().BeFalse();
        shipmentData.Should().BeNull();
    }

    [Fact]
    public void GetElementName_ReturnsCorrectName()
    {
        // Arrange
        var interchange = UniversalInterchangeHelper.CreateInterchange(new UniversalShipmentData
        {
            version = "1.1",
            Shipment = new Shipment()
        });

        // Act
        var elementName = interchange.GetElementName();

        // Assert
        elementName.Should().Be("UniversalShipment");
    }

    [Fact]
    public void DeserializeAndExtract_WorksEndToEnd()
    {
        // Arrange
        var shipmentData = new UniversalShipmentData
        {
            version = "1.1",
            Shipment = new Shipment()
        };
        var xml = UniversalInterchangeHelper.SerializeWithInterchange(shipmentData);

        // Act
        var extracted = UniversalInterchangeHelper.DeserializeAndExtract<UniversalShipmentData>(xml);

        // Assert
        extracted.Should().NotBeNull();
        extracted!.version.Should().Be("1.1");
        extracted.Shipment.Should().NotBeNull();
    }

    [Fact]
    public void TryDeserializeAndExtract_WithValidXml_ReturnsTrue()
    {
        // Arrange
        var shipmentData = new UniversalShipmentData
        {
            version = "1.1",
            Shipment = new Shipment()
        };
        var xml = UniversalInterchangeHelper.SerializeWithInterchange(shipmentData);

        // Act
        var success = UniversalInterchangeHelper.TryDeserializeAndExtract<UniversalShipmentData>(
            xml, out var extracted);

        // Assert
        success.Should().BeTrue();
        extracted.Should().NotBeNull();
        extracted!.version.Should().Be("1.1");
    }

    #region Helper Methods

    private static object? ExtractDataByType(UniversalInterchange interchange, Type type)
    {
        if (type == typeof(UniversalShipmentData))
            return interchange.ExtractData<UniversalShipmentData>();
        if (type == typeof(UniversalScheduleData))
            return interchange.ExtractData<UniversalScheduleData>();
        if (type == typeof(UniversalTransactionData))
            return interchange.ExtractData<UniversalTransactionData>();
        if (type == typeof(UniversalTransactionBatchData))
            return interchange.ExtractData<UniversalTransactionBatchData>();
        if (type == typeof(UniversalShipmentRequestData))
            return interchange.ExtractData<UniversalShipmentRequestData>();
        if (type == typeof(UniversalTransactionBatchRequestData))
            return interchange.ExtractData<UniversalTransactionBatchRequestData>();

        return null;
    }

    private static object CreateUniversalData(Type type)
    {
        if (type == typeof(UniversalShipmentData))
            return new UniversalShipmentData { version = "1.1", Shipment = new Shipment() };
        if (type == typeof(UniversalScheduleData))
            return new UniversalScheduleData { version = "1.1", Schedule = new Schedule() };
        if (type == typeof(UniversalTransactionData))
            return new UniversalTransactionData { version = "1.1", TransactionInfo = new TransactionInfo() };
        if (type == typeof(UniversalTransactionBatchData))
            return new UniversalTransactionBatchData { version = "1.1", TransactionBatch = new TransactionBatch() };
        if (type == typeof(UniversalShipmentRequestData))
            return new UniversalShipmentRequestData { version = "1.1", ShipmentRequest = new ShipmentRequest() };
        if (type == typeof(UniversalTransactionBatchRequestData))
            return new UniversalTransactionBatchRequestData { version = "1.1", TransactionBatchRequest = new TransactionBatchRequest() };

        throw new ArgumentException($"Unsupported type: {type.Name}");
    }

    #endregion Helper Methods
}
