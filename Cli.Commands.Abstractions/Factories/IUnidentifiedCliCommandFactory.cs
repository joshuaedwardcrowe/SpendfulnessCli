using Cli.Commands.Abstractions.Artefacts;
using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions.Factories;

public interface IUnidentifiedCliCommandFactory
{
    bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> properties) => true;
    
    CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties);
}