using System.Xml;
using CargoWiseNetLibrary.Models.Universal;

namespace CargoWiseNetLibrary.Serialization;

/// <summary>
/// Helper utilities for working with UniversalInterchange
/// </summary>
public static class UniversalInterchangeHelper
{
    /// <summary>
    /// Deserializes XML to UniversalInterchange and extracts the specific data type
    /// </summary>
    /// <typeparam name="T">The expected Universal data type (e.g., UniversalShipmentData)</typeparam>
    /// <param name="xml">The XML string to deserialize</param>
    /// <returns>The extracted data or null</returns>
    public static T? DeserializeAndExtract<T>(string xml) where T : class
    {
        var interchange = XmlSerializer<UniversalInterchange>.Deserialize(xml);
        return interchange?.ExtractData<T>();
    }

    /// <summary>
    /// Tries to deserialize XML and extract the specific data type
    /// </summary>
    /// <typeparam name="T">The expected Universal data type</typeparam>
    /// <param name="xml">The XML string to deserialize</param>
    /// <param name="data">The extracted data if successful</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool TryDeserializeAndExtract<T>(string xml, out T? data) where T : class
    {
        data = null;

        try
        {
            data = DeserializeAndExtract<T>(xml);
            return data != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Creates a UniversalInterchange wrapper around the given data
    /// </summary>
    /// <typeparam name="T">The Universal data type</typeparam>
    /// <param name="data">The data to wrap</param>
    /// <returns>A new UniversalInterchange instance</returns>
    public static UniversalInterchange CreateInterchange<T>(T data) where T : class
    {
        // Serialize using the runtime type to handle all Universal types correctly
        var serializer = new System.Xml.Serialization.XmlSerializer(data.GetType());
        using var stringWriter = new System.IO.StringWriter();
        using var xmlWriter = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Indent = false
        });
        serializer.Serialize(xmlWriter, data);
        var dataXml = stringWriter.ToString();

        // Parse to XmlElement
        var doc = new XmlDocument();
        doc.LoadXml(dataXml);

        return new UniversalInterchange
        {
            Body = new UniversalInterchangeBody
            {
                Any = { doc.DocumentElement! }
            }
        };
    }

    /// <summary>
    /// Serializes data by wrapping it in UniversalInterchange
    /// </summary>
    /// <typeparam name="T">The Universal data type</typeparam>
    /// <param name="data">The data to serialize</param>
    /// <param name="options">Optional XML serializer options</param>
    /// <returns>XML string</returns>
    public static string SerializeWithInterchange<T>(T data, XmlSerializerOptions? options = null) where T : class
    {
        var interchange = CreateInterchange(data);
        return XmlSerializer<UniversalInterchange>.Serialize(interchange, options);
    }
}

/// <summary>
/// Extension methods for UniversalInterchange
/// </summary>
public static class UniversalInterchangeExtensions
{
    /// <summary>
    /// Extracts the specific data type from the UniversalInterchange body
    /// </summary>
    /// <typeparam name="T">The expected data type</typeparam>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <returns>The extracted data or null</returns>
    public static T? ExtractData<T>(this UniversalInterchange? interchange) where T : class
    {
        if (interchange?.Body?.Any == null || interchange.Body.Any.Count == 0)
            return null;

        try
        {
            // Get the first XmlElement from the body
            var element = interchange.Body.Any[0];

            // Convert to XML string
            var xml = element.OuterXml;

            // Deserialize to the target type
            return XmlSerializer<T>.Deserialize(xml);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Tries to extract the specific data type from the UniversalInterchange body
    /// </summary>
    /// <typeparam name="T">The expected data type</typeparam>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <param name="data">The extracted data if successful</param>
    /// <returns>True if successful, false otherwise</returns>
    public static bool TryExtractData<T>(this UniversalInterchange? interchange, out T? data) where T : class
    {
        data = interchange.ExtractData<T>();
        return data != null;
    }

    /// <summary>
    /// Gets the type of data contained in the UniversalInterchange body by examining the root element name
    /// </summary>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <returns>The data type or null if no data found</returns>
    public static Type? GetDataType(this UniversalInterchange? interchange)
    {
        var elementName = interchange.GetElementName();
        if (elementName == null)
            return null;

        return elementName switch
        {
            "UniversalShipment" => typeof(UniversalShipmentData),
            "UniversalSchedule" => typeof(UniversalScheduleData),
            "UniversalTransaction" => typeof(UniversalTransactionData),
            "UniversalTransactionBatch" => typeof(UniversalTransactionBatchData),
            "UniversalShipmentRequest" => typeof(UniversalShipmentRequestData),
            "UniversalTransactionBatchRequest" => typeof(UniversalTransactionBatchRequestData),
            _ => null
        };
    }

    /// <summary>
    /// Gets the name of the data type contained in the UniversalInterchange body
    /// </summary>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <returns>The data type name or null if no data found</returns>
    public static string? GetDataTypeName(this UniversalInterchange? interchange)
    {
        return interchange.GetDataType()?.Name;
    }

    /// <summary>
    /// Gets the root element name from the body
    /// </summary>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <returns>The element name or null</returns>
    public static string? GetElementName(this UniversalInterchange? interchange)
    {
        if (interchange?.Body?.Any == null || interchange.Body.Any.Count == 0)
            return null;

        return interchange.Body.Any[0].LocalName;
    }

    /// <summary>
    /// Checks if the UniversalInterchange contains a specific data type
    /// </summary>
    /// <typeparam name="T">The data type to check for</typeparam>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <returns>True if the data type matches, false otherwise</returns>
    public static bool ContainsDataType<T>(this UniversalInterchange? interchange) where T : class
    {
        return interchange.GetDataType() == typeof(T);
    }

    /// <summary>
    /// Gets the data object from the UniversalInterchange body (untyped)
    /// </summary>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <returns>The data object or null</returns>
    public static object? GetData(this UniversalInterchange? interchange)
    {
        var dataType = interchange.GetDataType();
        if (dataType == null)
            return null;

        // Use reflection to call ExtractData<T> with the correct type
        var method = typeof(UniversalInterchangeExtensions)
            .GetMethod(nameof(ExtractData))!
            .MakeGenericMethod(dataType);

        return method.Invoke(null, [interchange]);
    }

    /// <summary>
    /// Checks if the UniversalInterchange has any data
    /// </summary>
    /// <param name="interchange">The UniversalInterchange instance</param>
    /// <returns>True if data exists, false otherwise</returns>
    public static bool HasData(this UniversalInterchange? interchange)
    {
        return interchange?.Body?.Any != null && interchange.Body.Any.Count > 0;
    }
}
