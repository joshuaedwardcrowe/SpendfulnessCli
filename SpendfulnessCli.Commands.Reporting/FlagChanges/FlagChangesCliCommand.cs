using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.FlagChanges;

public record FlagChangesCliCommand : CliCommand
{
    public static class ArgumentNames
    {
        public const string From = "from";
        public const string To = "to";
    }
    
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
}