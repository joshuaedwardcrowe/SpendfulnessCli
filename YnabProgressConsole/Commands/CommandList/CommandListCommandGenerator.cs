namespace YnabProgressConsole.Commands.CommandList;

public class CommandListCommandGenerator : ICommandGenerator
{
    public const string CommandName = "command-list";
    
    public ICommand Generate(List<string> arguments)
    {
        return new CommandListCommand();
    }
}