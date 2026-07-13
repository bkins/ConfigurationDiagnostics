using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal sealed class ProviderDumpStrategy : IConfigurationDumpStrategy
{
    public void Dump( IConfiguration           configuration
                    , TextWriter               writer
                    , ConfigurationDumpOptions options )
    {
        var configurationRoot = ConfigurationRootGuard.RequireRoot(configuration);

        ReportHeaderWriter.WriteHeading(writer
                                      , "Configuration Providers");

        foreach (var provider in configurationRoot.Providers)
        {
            writer.WriteLine($"\u2022 {ProviderDescriber.Describe(provider)}");
        }

        writer.WriteLine();
    }
}