using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands;

public interface ICommandGenerator
{
    ICommand Generate(List<InstructionArgument> arguments);
}