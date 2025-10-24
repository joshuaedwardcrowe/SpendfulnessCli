using Cli.Commands.Abstractions;
using Cli.Instructions;
using Microsoft.Extensions.DependencyInjection;

namespace Cli;

public class CliCommandProvider(IServiceProvider serviceProvider)
{
    public ICommand GetCommand(ConsoleInstruction instruction)
    {
        if (string.IsNullOrEmpty(instruction.Name))
        {
            throw new NoInstructionException("No instruction entered.");
        }
        
        var generator = serviceProvider.GetKeyedService<IGenericCommandGenerator>(instruction.Name);
        if (generator == null)
        {
            throw new NoCommandGeneratorException("Did not find generator for " + instruction.Name);
        }

        return generator.Generate(instruction.SubName, instruction.Arguments.ToList());
    }
}