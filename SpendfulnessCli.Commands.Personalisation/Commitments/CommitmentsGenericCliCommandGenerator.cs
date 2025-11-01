using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Personalisation.Commitments.Find;

namespace SpendfulnessCli.Commands.Personalisation.Commitments;

public class CommitmentsGenericCliCommandGenerator : ICliCommandGenerator<CommitmentsCliCommand>
{
    public CliCommand Generate(CliInstruction instruction) =>
        instruction.SubInstructionName switch
        {
            CommitmentsCliCommand.SubCommandNames.Find => new CommitmentFindCliCommand(),
            _ => new CommitmentsCliCommand(),
        };
}