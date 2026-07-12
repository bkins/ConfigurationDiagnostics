namespace ConfigurationDiagnostics;

/// <summary>
///     Controls how <see cref="ConfigurationExtensions" /> renders an
///     <see cref="Microsoft.Extensions.Configuration.IConfiguration" />.
///     Prefer adding new properties here over adding new extension method overloads.
/// </summary>
public sealed record ConfigurationDumpOptions
{
    public static readonly IReadOnlyCollection<string> DefaultSensitiveKeyFragments = new[]
                                                                                      {
                                                                                              "Password"
                                                                                            , "Secret"
                                                                                            , "ApiKey"
                                                                                            , "Token"
                                                                                            , "ConnectionString"
                                                                                            , "Credential"
                                                                                            , "AccessKey"
                                                                                            , "ClientSecret"
                                                                                      };

    /// <summary>Which strategy to run. Defaults to <see cref="ConfigurationDumpMode.Tree" />.</summary>
    public ConfigurationDumpMode Mode { get; init; } = ConfigurationDumpMode.Tree;

    /// <summary>
    ///     When true (the default), any key whose path contains one of <see cref="SensitiveKeyFragments" />
    ///     has its value replaced with a placeholder instead of being written out. Keep this on for any
    ///     dump that might land in shared logs, CI output, or screen recordings.
    /// </summary>
    public bool MaskSensitiveValues { get; init; } = true;

    /// <summary>When true, sections/keys are ordered alphabetically. When false, provider enumeration order is preserved.</summary>
    public bool SortAlphabetically { get; init; } = true;

    /// <summary>Maximum tree depth to descend into. Defaults to unlimited.</summary>
    public int MaxDepth { get; init; } = int.MaxValue;

    /// <summary>
    ///     Case-insensitive substrings checked against each key's full path to decide whether a value is sensitive.
    ///     Override this if your app has its own naming conventions for secrets (e.g. "Signing", "PrivateKey").
    /// </summary>
    public IReadOnlyCollection<string> SensitiveKeyFragments { get; init; } = DefaultSensitiveKeyFragments;
}