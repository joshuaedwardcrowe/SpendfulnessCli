using YnabCli.Commands.Database.Commitments.Find;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Database.Commitments;

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