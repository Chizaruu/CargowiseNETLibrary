using System.Text.RegularExpressions;

namespace CargoWiseNetLibrary.Tests.Utilities;

/// <summary>
/// Utility to extract all public model classes from Universal.cs for test generation
/// </summary>
public static class ModelExtractor
{
    /// <summary>
    /// Extracts all public class names from the Universal.cs file
    /// </summary>
    public static List<string> ExtractModelNames(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var content = File.ReadAllText(filePath);
        var modelNames = new List<string>();

        // Pattern to match public class declarations
        // Matches: public partial class ClassName
        var pattern = @"public\s+partial\s+class\s+(\w+)";
        var matches = Regex.Matches(content, pattern);

        foreach (Match match in matches)
        {
            if (match.Groups.Count > 1)
            {
                var className = match.Groups[1].Value;
                if (!modelNames.Contains(className))
                {
                    modelNames.Add(className);
                }
            }
        }

        return [.. modelNames.OrderBy(x => x)];
    }

    /// <summary>
    /// Extracts classes with XmlRoot attribute (top-level serializable models)
    /// </summary>
    public static List<string> ExtractRootModels(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var content = File.ReadAllText(filePath);
        var rootModels = new List<string>();

        // Pattern to match classes with XmlRoot attribute
        var pattern = @"\[XmlRootAttribute\([^)]+\)\]\s+public\s+partial\s+class\s+(\w+)";
        var matches = Regex.Matches(content, pattern);

        foreach (Match match in matches)
        {
            if (match.Groups.Count > 1)
            {
                var className = match.Groups[1].Value;
                if (!rootModels.Contains(className))
                {
                    rootModels.Add(className);
                }
            }
        }

        return [.. rootModels.OrderBy(x => x)];
    }

    /// <summary>
    /// Generates test data provider code for the extracted models
    /// </summary>
    public static string GenerateTestDataProvider(List<string> modelNames, string methodName = "GetModels")
    {
        var code = new System.Text.StringBuilder();
        code.AppendLine($"public static IEnumerable<object[]> {methodName}()");
        code.AppendLine("{");

        foreach (var modelName in modelNames)
        {
            code.AppendLine($"    yield return new object[] {{ new {modelName}(), \"{modelName}\" }};");
        }

        code.AppendLine("}");
        return code.ToString();
    }

    /// <summary>
    /// Generates a summary report of models in the file
    /// </summary>
    public static string GenerateReport(string filePath)
    {
        var allModels = ExtractModelNames(filePath);
        var rootModels = ExtractRootModels(filePath);

        var report = new System.Text.StringBuilder();
        report.AppendLine("=== Universal.cs Model Analysis ===");
        report.AppendLine();
        report.AppendLine($"Total Models: {allModels.Count}");
        report.AppendLine($"Root Models (with XmlRoot): {rootModels.Count}");
        report.AppendLine();

        report.AppendLine("Root Models:");
        foreach (var model in rootModels)
        {
            report.AppendLine($"  - {model}");
        }
        report.AppendLine();

        report.AppendLine($"All Models ({allModels.Count}):");

        // Group by first letter for readability
        var grouped = allModels.GroupBy(m => m[0]);
        foreach (var group in grouped.OrderBy(g => g.Key))
        {
            report.AppendLine($"  [{group.Key}] ({group.Count()} models):");
            foreach (var model in group.Take(10)) // Show first 10 per letter
            {
                report.AppendLine($"    - {model}");
            }
            if (group.Count() > 10)
            {
                report.AppendLine($"    ... and {group.Count() - 10} more");
            }
            report.AppendLine();
        }

        return report.ToString();
    }

    /// <summary>
    /// Saves extracted models to categorized files
    /// </summary>
    public static void SaveModelListsToFiles(string universalFilePath, string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        // Extract models
        var allModels = ExtractModelNames(universalFilePath);
        var rootModels = ExtractRootModels(universalFilePath);

        // Save root models
        var rootFile = Path.Combine(outputDirectory, "RootModels.txt");
        File.WriteAllLines(rootFile, rootModels);

        // Save all models
        var allFile = Path.Combine(outputDirectory, "AllModels.txt");
        File.WriteAllLines(allFile, allModels);

        // Generate and save test data provider
        var testDataFile = Path.Combine(outputDirectory, "GeneratedTestDataProvider.cs");
        File.WriteAllText(testDataFile, GenerateTestDataProvider(allModels));

        // Generate and save report
        var reportFile = Path.Combine(outputDirectory, "ModelAnalysis.txt");
        File.WriteAllText(reportFile, GenerateReport(universalFilePath));

        Console.WriteLine($"Files saved to: {outputDirectory}");
        Console.WriteLine($"  - RootModels.txt ({rootModels.Count} models)");
        Console.WriteLine($"  - AllModels.txt ({allModels.Count} models)");
        Console.WriteLine($"  - GeneratedTestDataProvider.cs");
        Console.WriteLine($"  - ModelAnalysis.txt");
    }
}
