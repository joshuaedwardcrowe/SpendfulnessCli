using YnabCli.Commands.Personalisation.Commitments.Find;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Personalisation.Commitments;

public class CommitmentCommandGenerator : ICommandGenerator, ITypedCommandGenerator<CommitmentCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        return subCommandName switch
        {
            CommitmentCommand.SubCommandNames.Find => new CommitmentFindCommand(),
            _ => new CommitmentCommand(),
        };
    }
}