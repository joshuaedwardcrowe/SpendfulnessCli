using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions.InstructionArgumentBuilders;

public class BoolInstructionArgumentBuilder : IInstructionArgumentBuilder
{
    public bool For(string argumentValue) => bool.TryParse(argumentValue, out _);

    public InstructionArgument Create(string argumentName, string argumentValue)
    {
        var argumentBool = bool.Parse(argumentValue);
        return new TypedInstructionArgument<bool>(argumentName, argumentBool);
    }
}