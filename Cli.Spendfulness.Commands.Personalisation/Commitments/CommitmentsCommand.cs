using Cli.Commands.Abstractions;

namespace Cli.Spendfulness.Commands.Personalisation.Commitments;

public class CommitmentsCommand : ICommand
{
    public static class SubCommandNames
    {
        public const string Find = "find";
    }
}