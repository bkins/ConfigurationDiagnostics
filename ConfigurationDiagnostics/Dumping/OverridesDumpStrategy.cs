using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal sealed class OverridesDumpStrategy : IConfigurationDumpStrategy
{
    public void Dump( IConfiguration           configuration
                    , TextWriter               writer
                    , ConfigurationDumpOptions options )
    {
        var configurationRoot = ConfigurationRootGuard.RequireRoot(configuration);

        ReportHeaderWriter.WriteHeading(writer
                                      , "Configuration Overrides");

        var configuredKeys = configurationRoot.AsEnumerable()
                                              .Where(entry => entry.Value != null)
                                              .Select(entry => entry.Key)
                                              .Distinct();

        var orderedKeys = options.SortAlphabetically
                                  ? configuredKeys.OrderBy(key => key)
                                  : configuredKeys;

        foreach (var key in orderedKeys)
        {
            WriteKeyOverrideHistory(configurationRoot
                                  , key
                                  , writer
                                  , options);
        }
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
            if ( ! provider.TryGet(key, out var value)) continue;
            
            finalValue = value;

            var displayValue = SensitiveValueMasker.Mask(key
                                                       , value
                                                       , options);
            writer.WriteLine($"    \u2022 {ProviderDescriber.Describe(provider)}");
            writer.WriteLine($"        {displayValue}");
        }

        var finalDisplayValue = SensitiveValueMasker.Mask(key
                                                        , finalValue
                                                        , options);
        writer.WriteLine("    Final");
        writer.WriteLine($"        {finalDisplayValue}");
        writer.WriteLine();
    }
}
