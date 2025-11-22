using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Personalisation.Commitments.Find;

namespace SpendfulnessCli.Commands.Personalisation.Commitments;

public class CommitmentsGenericCliCommandFactory : ICliCommandFactory<CommitmentsCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
        => instruction.SubInstructionName switch
        {
            CommitmentsCliCommand.SubCommandNames.Find => new CommitmentFindCliCommand(),
            _ => new CommitmentsCliCommand(),
        };
}