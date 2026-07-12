namespace ConfigurationDiagnostics;

internal static class SensitiveValueMasker
{
    private const string MaskedPlaceholder = "<Set MaskSensitiveValues = false in ConfigurationDumpOptions to see this value>";

    public static string? Mask( string                   keyPath
                              , string?                  value
                              , ConfigurationDumpOptions options )
    {
        if ( ! options.MaskSensitiveValues 
         || value == null)
        {
            return value;
        }

        return IsSensitiveKey(keyPath, options.SensitiveKeyFragments)
                       ? MaskedPlaceholder
                       : value;
    }

    private static bool IsSensitiveKey( string                      keyPath
                                      , IReadOnlyCollection<string> sensitiveKeyFragments )
    {
        return sensitiveKeyFragments.Any(fragment => keyPath.Contains(fragment
                                                                    , StringComparison.OrdinalIgnoreCase));
    }
}
