using System.Text;
using ConfigurationDiagnostics;
using Microsoft.Extensions.Configuration;

// Bullets (•) in the ByProvider/WithOverrides output need UTF-8 to render correctly in the
// console. This is an application-level concern, so it belongs here in the harness, not inside
// the library itself.
Console.OutputEncoding = Encoding.UTF8;

// Setting this before AddEnvironmentVariables() lets the demo show an env var overriding a value
// that also exists in appsettings.json, without you needing to set anything in your real OS
// environment first. Delete this line if you'd rather see the un-overridden value win instead.
Environment.SetEnvironmentVariable("CP_ExternalApi__ApiKey"
                                 , "env-override-secret-key");

var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
                                                                     {
                                                                             ["FeatureFlags:InMemoryOverride"] = "true"
                                                                     })
                                              .AddJsonFile("appsettings.json"
                                                         , optional: true
                                                         , reloadOnChange: false)
                                              .AddJsonFile("appsettings.Development.json"
                                                         , optional: true
                                                         , reloadOnChange: false)
                                              .AddEnvironmentVariables(prefix: "CP_")
                                              .Build();

PrintSectionBanner("TREE - the final effective configuration");
configuration.DumpToConsole(ConfigurationDumpMode.Tree);

PrintSectionBanner("BY PROVIDER - what got loaded, and in what order");
configuration.DumpToConsole(ConfigurationDumpMode.ByProvider);

PrintSectionBanner("WITH OVERRIDES - which provider won for each key");
configuration.DumpToConsole(ConfigurationDumpMode.WithOverrides);

PrintSectionBanner("DETAILED - all three combined");
configuration.DumpToConsole(ConfigurationDumpMode.Detailed);

PrintSectionBanner("CUSTOM OPTIONS - masking turned off (only ever do this locally!)");
configuration.DumpToConsole(new ConfigurationDumpOptions
                            {
                                    Mode                = ConfigurationDumpMode.Detailed
                                  , MaskSensitiveValues = false
                            });

PrintSectionBanner("DumpToString - captured as a string instead of printed, like a test would use it");

var treeSnapshot         = configuration.DumpToString(new ConfigurationDumpOptions { Mode = ConfigurationDumpMode.Tree });
var containsMaskedMarker = treeSnapshot.Contains("***MASKED***");

Console.WriteLine($"Captured {treeSnapshot.Length} characters. Contains masked marker: {containsMaskedMarker}");

return;

static void PrintSectionBanner( string title )
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 80));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 80));
}