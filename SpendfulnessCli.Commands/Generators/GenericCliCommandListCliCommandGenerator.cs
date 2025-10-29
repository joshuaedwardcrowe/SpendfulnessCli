using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Generators;

public class GenericCliCommandListCliCommandGenerator : ICliCommandGenerator<CommandListCliCommand>
{
    public ICliCommand Generate(CliInstruction instruction)
    {
        return new CommandListCliCommand();
    }
}