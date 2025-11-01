using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Generators;

public class GenericCliCommandListCliCommandGenerator : ICliCommandGenerator<CommandListCliCommand>
{
    public CliCommand Generate(CliInstruction instruction) => new CommandListCliCommand();
}