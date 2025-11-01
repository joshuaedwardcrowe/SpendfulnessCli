using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;

namespace Cli.Workflow.Commands;

public class ExitCliCommandGenerator : ICliCommandGenerator<ExitCliCommand>
{
    public CliCommand Generate(CliInstruction instruction) => new ExitCliCommand();
}