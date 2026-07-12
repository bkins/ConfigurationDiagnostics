using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal sealed class OverridesDumpStrategy : IConfigurationDumpStrategy
{
    public void Dump( IConfiguration           configuration
                    , TextWriter               writer
                    , ConfigurationDumpOptions options )
    {
        var configurationRoot = ConfigurationRootGuard.RequireRoot(configuration);

        writer.WriteLine();
        writer.WriteLine("Configuration Overrides");
        writer.WriteLine(new string(c: '-'
                                  , count: 60));

        var configuredKeys = configurationRoot.AsEnumerable()
                                              .Where(entry => entry.Value != null)
                                              .Select(entry => entry.Key)
                                              .Distinct();

        var orderedKeys = options.SortAlphabetically
                                  ? configuredKeys.OrderBy(key => key)
                                  : configuredKeys;

        foreach (var key in orderedKeys)
            WriteKeyOverrideHistory(configurationRoot: configurationRoot
                                  , key: key
                                  , writer: writer
                                  , options: options);
    }

    private static void WriteKeyOverrideHistory( IConfigurationRoot       configurationRoot
                                               , string                   key
                                               , TextWriter               writer
                                               , ConfigurationDumpOptions options )
    {
        writer.WriteLine(key);

        string? finalValue = null;

        foreach (var provider in configurationRoot.Providers)
        {
            if (!provider.TryGet(key: key
                               , value: out var value)) continue;

            finalValue = value;

            var displayValue = SensitiveValueMasker.Mask(keyPath: key
                                                       , value: value
                                                       , options: options);
            writer.WriteLine($"    {provider}");
            writer.WriteLine($"        {displayValue}");
        }

        var finalDisplayValue = SensitiveValueMasker.Mask(keyPath: key
                                                        , value: finalValue
                                                        , options: options);
        writer.WriteLine("    Final");
        writer.WriteLine($"        {finalDisplayValue}");
        writer.WriteLine();
    }
}