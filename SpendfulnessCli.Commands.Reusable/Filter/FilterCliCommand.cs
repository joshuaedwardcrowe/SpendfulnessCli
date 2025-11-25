using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Reusable.Filter;

public record FilterCliCommand(string FilterOn) : CliCommand
{
    public static class ArgumentNames
    {
        public const string FilterOn = "on";
    }
}