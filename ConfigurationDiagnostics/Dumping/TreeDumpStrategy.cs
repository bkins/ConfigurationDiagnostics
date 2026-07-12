using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal sealed class TreeDumpStrategy : IConfigurationDumpStrategy
{
    public void Dump( IConfiguration           configuration
                    , TextWriter               writer
                    , ConfigurationDumpOptions options )
    {
        writer.WriteLine();
        writer.WriteLine("Effective Configuration");
        writer.WriteLine(new string(c: '-'
                                  , count: 60));

        PrintSections(sections: configuration.GetChildren()
                    , writer: writer
                    , options: options
                    , depth: 0);

        writer.WriteLine();
    }

    private static void PrintSections( IEnumerable<IConfigurationSection> sections
                                     , TextWriter                         writer
                                     , ConfigurationDumpOptions           options
                                     , int                                depth )
    {
        if (depth > options.MaxDepth) return;

        var orderedSections = options.SortAlphabetically
                                      ? sections.OrderBy(section => section.Key)
                                      : sections;

        foreach (var section in orderedSections)
        {
            var childSections = section.GetChildren().ToList();
            var indent = new string(c: ' '
                                  , count: depth * 2);

            if (childSections.Count > 0)
            {
                writer.WriteLine($"{indent}{section.Key}");
                PrintSections(sections: childSections
                            , writer: writer
                            , options: options
                            , depth: depth + 1);
            }
            else
            {
                var displayValue = SensitiveValueMasker.Mask(keyPath: section.Path
                                                           , value: section.Value
                                                           , options: options);
                writer.WriteLine($"{indent}{section.Key}: {displayValue}");
            }
        }
    }
}