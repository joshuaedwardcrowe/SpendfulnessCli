
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions.InstructionArgumentBuilders;

public class StringInstructionArgumentBuilder : IInstructionArgumentBuilder
{
    public bool For(string argumentValue)
        => argumentValue
            .ToCharArray()
            .Where(x => !char.IsWhiteSpace(x))
            .All(char.IsLetter);

    public InstructionArgument Create(string argumentName, string argumentValue)
    {
        return new TypedInstructionArgument<string>
        {
            ArgumentName = argumentName,
            ArgumentValue = argumentValue
        };
    }
}