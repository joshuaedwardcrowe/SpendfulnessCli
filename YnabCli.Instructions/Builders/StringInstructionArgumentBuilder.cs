using YnabCli.Instructions.Arguments;
using YnabCli.Instructions.Extensions;

namespace YnabCli.Instructions.Builders;

public class StringInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, IInstructionArgumentBuilder
{
    public bool For(string? argumentValue)
    {
        if (argumentValue == null) return false;

        return argumentValue.AnyLetters() && !bool.TryParse(argumentValue, out _);
    }

    public InstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        return new TypedInstructionArgument<string>(argumentName, validArgumentValue);
    }
}