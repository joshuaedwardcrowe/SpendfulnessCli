using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Instructions.Extensions;

namespace Cli.Instructions.Builders;

public class StringCliInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue)
    {
        if (argumentValue == null) return false;

        return argumentValue.AnyLetters() && !bool.TryParse(argumentValue, out _);
    }

    public ConsoleInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        return new TypedConsoleInstructionArgument<string>(argumentName, validArgumentValue);
    }
}