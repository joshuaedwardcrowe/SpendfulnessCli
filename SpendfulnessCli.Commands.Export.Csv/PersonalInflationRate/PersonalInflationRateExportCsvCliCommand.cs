using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Export.Csv.PersonalInflationRate;

public record PersonalInflationRateExportCsvCliCommand(DirectoryInfo OutputFileSystemInfo) : CliCommand
{
    public static class ArgumentNames
    {
        public const string OutputFileSystemPath = "output-to";
    }
}