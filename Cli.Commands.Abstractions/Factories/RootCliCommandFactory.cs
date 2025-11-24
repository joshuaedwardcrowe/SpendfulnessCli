using Cli.Commands.Abstractions.Artefacts;
using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions.Factories;

public abstract class RootCliCommandFactory
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts) => false;
}