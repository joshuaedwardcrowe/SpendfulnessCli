using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Spendfulness.Commands.Personalisation.Commitments.Find;

namespace Cli.Spendfulness.Commands.Personalisation.Commitments;

public class CommitmentsGenericCommandGenerator : ICommandGenerator<CommitmentsCliCommand>
{
    public ICliCommand Generate(CliInstruction instruction) =>
        instruction.SubInstructionName switch
        {
            CommitmentsCliCommand.SubCommandNames.Find => new CommitmentFindCliCommand(),
            _ => new CommitmentsCliCommand(),
        };
}