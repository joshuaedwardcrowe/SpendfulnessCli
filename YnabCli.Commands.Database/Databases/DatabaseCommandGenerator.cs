using YnabCli.Commands.Database.Databases.Create;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Database.Databases;

public class DatabaseCommandGenerator : ICommandGenerator, ITypedCommandGenerator<DatabaseCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        if (subCommandName == DatabaseCommand.SubCommandNames.Create)
        {
            return new DatabaseCreateCommand();
        }
        
        return new DatabaseCommand();
    }
}