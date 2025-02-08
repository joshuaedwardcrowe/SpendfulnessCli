using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.CommandList;

public class CommandListCommandGenerator : ICommandGenerator
{
    // TODO: Move to the command.
    public const string CommandName = "command-list";
    
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        return new CommandListCommand();
    }
}