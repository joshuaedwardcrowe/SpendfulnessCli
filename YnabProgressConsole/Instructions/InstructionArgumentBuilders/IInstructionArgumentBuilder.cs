using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions.InstructionArgumentBuilders;

public interface IInstructionArgumentBuilder
{
    bool For(string argumentValue);

    InstructionArgument Create(string argumentName, string argumentValue);
}