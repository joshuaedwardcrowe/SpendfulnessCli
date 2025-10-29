using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Personalisation.Commitments;

public class CommitmentsCliCommand : ICliCommand
{
    public static class SubCommandNames
    {
        public const string Find = "find";
    }
}