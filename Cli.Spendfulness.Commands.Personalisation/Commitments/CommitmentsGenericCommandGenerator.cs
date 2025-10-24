using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Spendfulness.Commands.Personalisation.Commitments.Find;

namespace Cli.Spendfulness.Commands.Personalisation.Commitments;

public class CommitmentsGenericCommandGenerator : ICommandGenerator<CommitmentsCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        return subCommandName switch
        {
            CommitmentsCommand.SubCommandNames.Find => new CommitmentFindCommand(),
            _ => new CommitmentsCommand(),
        };
    }
}