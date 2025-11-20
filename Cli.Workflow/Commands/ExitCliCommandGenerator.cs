using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;

namespace Cli.Workflow.Commands;

public class ExitCliCommandGenerator : ICliCommandGenerator<ExitCliCommand>
{
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
        => new ExitCliCommand();
}