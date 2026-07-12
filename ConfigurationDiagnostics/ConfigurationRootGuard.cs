using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics;

internal static class ConfigurationRootGuard
{
    public static IConfigurationRoot RequireRoot( IConfiguration configuration )
    {
        if (configuration is IConfigurationRoot configurationRoot) return configurationRoot;

        throw new InvalidOperationException(
            "This dump mode requires an IConfigurationRoot (typically the root IConfiguration registered by "
          + "the host) because it needs access to the underlying configuration providers. Tree mode does not "
          + "have this requirement and works with any IConfiguration, including scoped sections.");
    }
}