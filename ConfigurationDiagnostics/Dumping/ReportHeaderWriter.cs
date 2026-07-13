namespace ConfigurationDiagnostics.Dumping;

internal static class ReportHeaderWriter
{
    public static void WriteHeading(TextWriter writer, string heading, char underline = '-')
    {
        writer.WriteLine();
        writer.WriteLine(heading);
        writer.WriteLine(new string(underline, heading.Length));
    }
}