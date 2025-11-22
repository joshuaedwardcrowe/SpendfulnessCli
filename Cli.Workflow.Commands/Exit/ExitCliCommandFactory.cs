using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;

namespace Cli.Workflow.Commands.Exit;

public class ExitCliCommandFactory : ICliCommandFactory<ExitCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
        => new ExitCliCommand();
}