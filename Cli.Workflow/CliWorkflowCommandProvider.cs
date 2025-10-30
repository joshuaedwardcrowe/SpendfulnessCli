using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Exceptions;
using Cli.Instructions.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow;

// TODO: CLI - I think this abstraction is unnecessary.
public class CliWorkflowCommandProvider(IServiceProvider serviceProvider)
{
    public CliCommand Provide(CliInstruction instruction)
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

    public CliCommand Provide(CliInstruction priorInstruction, CliInstruction nextInstruction)
    {
        if (string.IsNullOrEmpty(priorInstruction.Name))
        {
            throw new NoInstructionException("No instruction entered.");
        }
        
        var priorCommandGenerator = serviceProvider.GetKeyedService<IUnidentifiedCliCommandGenerator>(priorInstruction.Name);
        if (priorCommandGenerator == null)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + nextInstruction.Name);
        }
        
        var builder = new CliNextCommmandDefinition();
        
        priorCommandGenerator.NextCommands(builder);
        
        if (!builder.CanMoveTo(nextInstruction.Name!))
        {
            // TODO: Better exception
            throw new NotImplementedException();
        }
        
        var nextCommandGenerator = serviceProvider.GetKeyedService<IUnidentifiedCliCommandGenerator>(nextInstruction.Name);
        if (nextCommandGenerator == null)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + nextInstruction.Name);
        }
        
        return nextCommandGenerator.Generate(nextInstruction);
    }
}