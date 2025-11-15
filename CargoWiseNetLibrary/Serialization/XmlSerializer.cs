using System.Xml;
using SystemXmlSerializer = System.Xml.Serialization.XmlSerializer;

namespace CargoWiseNetLibrary.Serialization;

/// <summary>
/// Generic XML serialization helper for CargoWise models
/// </summary>
/// <typeparam name="T">The type to serialize/deserialize</typeparam>
public static class XmlSerializer<T> where T : class
{
    private static readonly SystemXmlSerializer Serializer = new(typeof(T));

    /// <summary>
    /// Serializes an object to XML string
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <param name="options">Optional XML serializer options</param>
    /// <returns>XML string representation</returns>
    public static string Serialize(T obj, XmlSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        options ??= new XmlSerializerOptions();

        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
        {
            Indent = options.Indent,
            OmitXmlDeclaration = options.OmitXmlDeclaration,
            Encoding = System.Text.Encoding.UTF8
        });

        Serializer.Serialize(xmlWriter, obj);
        return stringWriter.ToString();
    }

    /// <summary>
    /// Deserializes XML string to object
    /// </summary>
    /// <param name="xml">The XML string to deserialize</param>
    /// <returns>Deserialized object or null</returns>
    public static T? Deserialize(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return null;

        using var stringReader = new StringReader(xml);
        return Serializer.Deserialize(stringReader) as T;
    }

    /// <summary>
    /// Serializes an object to an XML file
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <param name="filePath">The file path to write to</param>
    /// <param name="options">Optional XML serializer options</param>
    public static void SerializeToFile(T obj, string filePath, XmlSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var xml = Serialize(obj, options);
        File.WriteAllText(filePath, xml);
    }

    /// <summary>
    /// Serializes an object to an XML file asynchronously
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <param name="filePath">The file path to write to</param>
    /// <param name="options">Optional XML serializer options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task SerializeToFileAsync(
        T obj,
        string filePath,
        XmlSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var xml = Serialize(obj, options);
        await File.WriteAllTextAsync(filePath, xml, cancellationToken);
    }

    /// <summary>
    /// Deserializes an object from an XML file
    /// </summary>
    /// <param name="filePath">The file path to read from</param>
    /// <returns>Deserialized object or null</returns>
    public static T? DeserializeFromFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var xml = File.ReadAllText(filePath);
        return Deserialize(xml);
    }

    /// <summary>
    /// Deserializes an object from an XML file asynchronously
    /// </summary>
    /// <param name="filePath">The file path to read from</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deserialized object or null</returns>
    public static async Task<T?> DeserializeFromFileAsync(
        string filePath,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var xml = await File.ReadAllTextAsync(filePath, cancellationToken);
        return Deserialize(xml);
    }

    /// <summary>
    /// Tries to deserialize XML string to object
    /// </summary>
    /// <param name="xml">The XML string to deserialize</param>
    /// <param name="result">The deserialized object if successful</param>
    /// <returns>True if deserialization succeeded, false otherwise</returns>
    public static bool TryDeserialize(string xml, out T? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(xml))
            return false;

        try
        {
            result = Deserialize(xml);
            return result != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates that an XML string can be deserialized to the target type
    /// </summary>
    /// <param name="xml">The XML string to validate</param>
    /// <returns>True if XML is valid for the type, false otherwise</returns>
    public static bool IsValid(string xml)
    {
        return TryDeserialize(xml, out _);
    }

    /// <summary>
    /// Creates a deep clone of an object using XML serialization
    /// </summary>
    /// <param name="obj">The object to clone</param>
    /// <param name="options">Optional XML serializer options</param>
    /// <returns>Cloned object</returns>
    public static T Clone(T obj, XmlSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var xml = Serialize(obj, options);
        return Deserialize(xml)
            ?? throw new InvalidOperationException("Failed to clone object - deserialization returned null");
    }
}

/// <summary>
/// Options for XML serialization
/// </summary>
public class XmlSerializerOptions
{
    /// <summary>
    /// Gets or sets whether to indent the XML output
    /// </summary>
    public bool Indent { get; set; } = true;

    /// <summary>
    /// Gets or sets whether to omit the XML declaration
    /// </summary>
    public bool OmitXmlDeclaration { get; set; } = false;

    /// <summary>
    /// Gets or sets the XML namespace (currently not used, reserved for future use)
    /// </summary>
    public string? Namespace { get; set; }
}
