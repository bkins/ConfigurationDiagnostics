using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal sealed class ProviderDumpStrategy : IConfigurationDumpStrategy
{
    public void Dump( IConfiguration           configuration
                    , TextWriter               writer
                    , ConfigurationDumpOptions options )
    {
        var configurationRoot = ConfigurationRootGuard.RequireRoot(configuration);

        writer.WriteLine();
        writer.WriteLine("Configuration Providers");
        writer.WriteLine(new string(c: '-'
                                  , count: 60));

        foreach (var provider in configurationRoot.Providers) writer.WriteLine(provider);

        writer.WriteLine();
    }
}