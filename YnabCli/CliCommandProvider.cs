using Cli.Instructions;
using Microsoft.Extensions.DependencyInjection;
using YnabCli.Commands;
using YnabCli.Commands.Generators;

namespace YnabCli;

public class CliCommandProvider(IServiceProvider serviceProvider)
{
    public ICommand GetCommand(ConsoleInstruction instruction)
    {
        if (string.IsNullOrEmpty(instruction.Name))
        {
            // TODO: Custom command.
            throw new Exception("No instruction entered.");
        }
        
        var generator = serviceProvider.GetKeyedService<IGenericCommandGenerator>(instruction.Name);
        if (generator == null)
        {
            // TODO: Custom command.
            throw new Exception("Did not find generator for " + instruction.Name);
        }

        return generator.Generate(instruction.SubName, instruction.Arguments.ToList());
    }
}