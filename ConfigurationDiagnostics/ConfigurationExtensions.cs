using Microsoft.Extensions.Configuration;

namespace ConfigurationDiagnostics;

public static class ConfigurationExtensions
{
    /// <summary>Writes the configuration dump to <see cref="Console.Out"/> using the given mode.</summary>
    public static void DumpToConsole(
        this IConfiguration configuration,
        ConfigurationDumpMode mode = ConfigurationDumpMode.Tree)
    {
        configuration.DumpToConsole(new ConfigurationDumpOptions { Mode = mode });
    }

    /// <summary>Writes the configuration dump to <see cref="Console.Out"/> using full options.</summary>
    public static void DumpToConsole(
        this IConfiguration configuration,
        ConfigurationDumpOptions options)
    {
        ConfigurationDumper.Dump(configuration, Console.Out, options);
    }

    /// <summary>Writes the configuration dump to an arbitrary <see cref="TextWriter"/>, e.g. a log sink or file stream.</summary>
    public static void DumpToWriter(
        this IConfiguration configuration,
        TextWriter writer,
        ConfigurationDumpOptions? options = null)
    {
        ConfigurationDumper.Dump(configuration, writer, options ?? new ConfigurationDumpOptions());
    }

    /// <summary>
    /// Renders the configuration dump to a string instead of writing it anywhere. Useful in unit tests
    /// (assert against the returned string) and for embedding a snapshot in an error report or health check.
    /// </summary>
    public static string DumpToString(
        this IConfiguration configuration,
        ConfigurationDumpOptions? options = null)
    {
        using var stringWriter = new StringWriter();
        ConfigurationDumper.Dump(configuration, stringWriter, options ?? new ConfigurationDumpOptions());
        return stringWriter.ToString();
    }
}
