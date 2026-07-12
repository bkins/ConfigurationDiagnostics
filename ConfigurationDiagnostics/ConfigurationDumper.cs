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
            [ConfigurationDumpMode.Tree] = new TreeDumpStrategy(),
            [ConfigurationDumpMode.ByProvider] = new ProviderDumpStrategy(),
            [ConfigurationDumpMode.WithOverrides] = new OverridesDumpStrategy(),
            [ConfigurationDumpMode.Detailed] = new DetailedDumpStrategy(
                providerDumpStrategy: new ProviderDumpStrategy(),
                treeDumpStrategy: new TreeDumpStrategy(),
                overridesDumpStrategy: new OverridesDumpStrategy())
        };

    public static void Dump(IConfiguration configuration, TextWriter writer, ConfigurationDumpOptions options)
    {
        if (!Strategies.TryGetValue(options.Mode, out var strategy))
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                options.Mode,
                "Unhandled configuration dump mode.");
        }

        strategy.Dump(configuration, writer, options);
    }
}
