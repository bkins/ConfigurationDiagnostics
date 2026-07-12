using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics.Dumping;

/// <summary>
/// One rendering strategy for a <see cref="ConfigurationDumpMode"/>. Implementations that need
/// provider-level access should call <see cref="ConfigurationRootGuard.RequireRoot"/> themselves
/// rather than assuming the caller already validated the type - that keeps each strategy honest
/// about its own requirements instead of leaning on a blanket check upstream.
/// </summary>
internal interface IConfigurationDumpStrategy
{
    void Dump(IConfiguration configuration, TextWriter writer, ConfigurationDumpOptions options);
}
