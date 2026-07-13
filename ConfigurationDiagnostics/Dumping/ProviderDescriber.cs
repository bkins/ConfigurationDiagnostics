using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

internal static class ProviderDescriber
{
    public static string Describe(IConfigurationProvider provider)
    {
        var description  = provider.ToString();
        var providerType = provider.GetType();

        var toStringWasNotOverridden = description == providerType.ToString();

        return toStringWasNotOverridden
                       ? providerType.Name
                       : description ?? providerType.Name;
    }
}