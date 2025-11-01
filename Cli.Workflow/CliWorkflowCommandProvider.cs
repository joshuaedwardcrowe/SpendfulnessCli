using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Exceptions;
using Cli.Instructions.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow;

// TODO: CLI - I think this abstraction is unnecessary.
public class CliWorkflowCommandProvider(IServiceProvider serviceProvider)
{
    public CliCommand GetCommand(CliInstruction instruction)
    {
        if (string.IsNullOrEmpty(instruction.Name))
        {
            throw new NoInstructionException("No instruction entered.");
        }
        
        var generator = serviceProvider.GetKeyedService<IUnidentifiedCliCommandGenerator>(instruction.Name);
        if (generator == null)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + instruction.Name);
        }

        return generator.Generate(instruction);
    }
}