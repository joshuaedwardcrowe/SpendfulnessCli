using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.CommandList;

public class CommandListCommandGenerator : ICommandGenerator, ITypedCommandGenerator<CommandListCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        return new CommandListCommand();
    }
}