using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Spendfulness.Commands.Personalisation.Databases.Create;

namespace Cli.Spendfulness.Commands.Personalisation.Databases;

public class DatabaseGenericCommandGenerator : ICommandGenerator<DatabaseCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        if (subCommandName == DatabaseCommand.SubCommandNames.Create)
        {
            return new DatabaseCreateCommand();
        }
        
        return new DatabaseCommand();
    }
}