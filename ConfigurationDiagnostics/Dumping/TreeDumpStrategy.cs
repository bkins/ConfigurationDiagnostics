using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal sealed class TreeDumpStrategy : IConfigurationDumpStrategy
{
    public void Dump(IConfiguration configuration, TextWriter writer, ConfigurationDumpOptions options)
    {
        writer.WriteLine();
        writer.WriteLine("Effective Configuration");
        writer.WriteLine(new string('-', 60));

        PrintSections(configuration.GetChildren(), writer, options, depth: 0);

        writer.WriteLine();
    }

    private static void PrintSections(
        IEnumerable<IConfigurationSection> sections,
        TextWriter writer,
        ConfigurationDumpOptions options,
        int depth)
    {
        if (depth > options.MaxDepth)
        {
            return;
        }

        var orderedSections = options.SortAlphabetically
            ? sections.OrderBy(section => section.Key)
            : sections;

        foreach (var section in orderedSections)
        {
            var childSections = section.GetChildren().ToList();
            var indent = new string(' ', depth * 2);

            if (childSections.Count > 0)
            {
                writer.WriteLine($"{indent}{section.Key}");
                PrintSections(childSections, writer, options, depth + 1);
            }
            else
            {
                var displayValue = SensitiveValueMasker.Mask(section.Path, section.Value, options);
                writer.WriteLine($"{indent}{section.Key}: {displayValue}");
            }
        }
    }
}
