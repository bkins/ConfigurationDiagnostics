using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal sealed class DetailedDumpStrategy : IConfigurationDumpStrategy
{
    private readonly IConfigurationDumpStrategy _providerDumpStrategy;
    private readonly IConfigurationDumpStrategy _treeDumpStrategy;
    private readonly IConfigurationDumpStrategy _overridesDumpStrategy;

    public DetailedDumpStrategy(
        IConfigurationDumpStrategy providerDumpStrategy,
        IConfigurationDumpStrategy treeDumpStrategy,
        IConfigurationDumpStrategy overridesDumpStrategy)
    {
        _providerDumpStrategy = providerDumpStrategy;
        _treeDumpStrategy = treeDumpStrategy;
        _overridesDumpStrategy = overridesDumpStrategy;
    }

    public void Dump(IConfiguration configuration, TextWriter writer, ConfigurationDumpOptions options)
    {
        var configurationRoot = ConfigurationRootGuard.RequireRoot(configuration);

        writer.WriteLine();
        writer.WriteLine("Configuration Diagnostic Report");
        writer.WriteLine(new string('=', 60));

        var configuredKeyCount = configurationRoot.AsEnumerable().Count(entry => entry.Value != null);

        writer.WriteLine($"Providers Loaded   : {configurationRoot.Providers.Count()}");
        writer.WriteLine($"Configuration Keys : {configuredKeyCount}");

        _providerDumpStrategy.Dump(configurationRoot, writer, options);
        _treeDumpStrategy.Dump(configurationRoot, writer, options);
        _overridesDumpStrategy.Dump(configurationRoot, writer, options);
    }
}
