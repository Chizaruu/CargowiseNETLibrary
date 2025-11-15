using System.Text.Json;
using SystemJsonSerializer = System.Text.Json.JsonSerializer;

namespace CargoWiseNetLibrary.Serialization;

/// <summary>
/// Shared JSON serialization settings
/// </summary>
internal static class JsonSerializerDefaults
{
    internal static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };
}

/// <summary>
/// Generic JSON serialization helper for CargoWise models
/// </summary>
/// <typeparam name="T">The type to serialize/deserialize</typeparam>
public static class JsonSerializer<T> where T : class
{
    /// <summary>
    /// Serializes an object to JSON string
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <returns>JSON string representation</returns>
    public static string Serialize(T obj, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return SystemJsonSerializer.Serialize(obj, options ?? JsonSerializerDefaults.DefaultOptions);
    }

    /// <summary>
    /// Deserializes JSON string to object
    /// </summary>
    /// <param name="json">The JSON string to deserialize</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <returns>Deserialized object or null</returns>
    public static T? Deserialize(string json, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        return SystemJsonSerializer.Deserialize<T>(json, options ?? JsonSerializerDefaults.DefaultOptions);
    }

    /// <summary>
    /// Serializes an object to a JSON file
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <param name="filePath">The file path to write to</param>
    /// <param name="options">Optional JSON serializer options</param>
    public static void SerializeToFile(T obj, string filePath, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        var json = Serialize(obj, options);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// Serializes an object to a JSON file asynchronously
    /// </summary>
    /// <param name="obj">The object to serialize</param>
    /// <param name="filePath">The file path to write to</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task SerializeToFileAsync(
        T obj,
        string filePath,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        await using var fileStream = File.Create(filePath);
        await SystemJsonSerializer.SerializeAsync(fileStream, obj, options ?? JsonSerializerDefaults.DefaultOptions, cancellationToken);
    }

    /// <summary>
    /// Deserializes an object from a JSON file
    /// </summary>
    /// <param name="filePath">The file path to read from</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <returns>Deserialized object or null</returns>
    public static T? DeserializeFromFile(string filePath, JsonSerializerOptions? options = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var json = File.ReadAllText(filePath);
        return Deserialize(json, options);
    }

    /// <summary>
    /// Deserializes an object from a JSON file asynchronously
    /// </summary>
    /// <param name="filePath">The file path to read from</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Deserialized object or null</returns>
    public static async Task<T?> DeserializeFromFileAsync(
        string filePath,
        JsonSerializerOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        await using var fileStream = File.OpenRead(filePath);
        return await SystemJsonSerializer.DeserializeAsync<T>(fileStream, options ?? JsonSerializerDefaults.DefaultOptions, cancellationToken);
    }

    /// <summary>
    /// Creates a deep clone of an object using JSON serialization
    /// </summary>
    /// <param name="obj">The object to clone</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <returns>Cloned object</returns>
    public static T Clone(T obj, JsonSerializerOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var json = Serialize(obj, options);
        return Deserialize(json, options)
            ?? throw new InvalidOperationException("Failed to clone object - deserialization returned null");
    }

    /// <summary>
    /// Tries to deserialize JSON string to object
    /// </summary>
    /// <param name="json">The JSON string to deserialize</param>
    /// <param name="result">The deserialized object if successful</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <returns>True if deserialization succeeded, false otherwise</returns>
    public static bool TryDeserialize(
        string json,
        out T? result,
        JsonSerializerOptions? options = null)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            result = Deserialize(json, options);
            return result != null;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates that a JSON string can be deserialized to the target type
    /// </summary>
    /// <param name="json">The JSON string to validate</param>
    /// <param name="options">Optional JSON serializer options</param>
    /// <returns>True if JSON is valid for the type, false otherwise</returns>
    public static bool IsValid(string json, JsonSerializerOptions? options = null)
    {
        return TryDeserialize(json, out _, options);
    }
}
