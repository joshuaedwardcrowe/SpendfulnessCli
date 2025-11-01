using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Commitments;

public record CommitmentsCliCommand : CliCommand
{
    public static class SubCommandNames
    {
        public const string Find = "find";
    }
}