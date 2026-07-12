using ConfigurationDiagnostics.Dumping;
using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics;

internal static class ConfigurationDumper
{
    // Adding a new ConfigurationDumpMode means adding an entry here and a new strategy class -
    // no existing strategy needs to change. If you want true open/closed extensibility (e.g. letting
    // a consuming app register its own custom mode), swap this for a strategy resolved via DI instead.
    private static readonly IReadOnlyDictionary<ConfigurationDumpMode, IConfigurationDumpStrategy> Strategies =
            new Dictionary<ConfigurationDumpMode, IConfigurationDumpStrategy>
            {
                    [ConfigurationDumpMode.Tree]          = new TreeDumpStrategy(), [ConfigurationDumpMode.ByProvider] = new ProviderDumpStrategy()
                  , [ConfigurationDumpMode.WithOverrides] = new OverridesDumpStrategy(), [ConfigurationDumpMode.Detailed] = new DetailedDumpStrategy(
                        providerDumpStrategy: new ProviderDumpStrategy()
                      , treeDumpStrategy: new TreeDumpStrategy()
                      , overridesDumpStrategy: new OverridesDumpStrategy())
            };

    public static void Dump( IConfiguration           configuration
                           , TextWriter               writer
                           , ConfigurationDumpOptions options )
    {
        if (!Strategies.TryGetValue(key: options.Mode
                                  , value: out var strategy))
            throw new ArgumentOutOfRangeException(
                paramName: nameof(options)
              , actualValue: options.Mode
              , message: "Unhandled configuration dump mode.");

        strategy.Dump(configuration: configuration
                    , writer: writer
                    , options: options);
    }
}