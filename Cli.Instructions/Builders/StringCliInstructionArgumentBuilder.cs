using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using Cli.Instructions.Extensions;

namespace Cli.Instructions.Builders;

// TODO: Write unit tests.
internal class StringCliInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue)
    {
        if (argumentValue == null) return false;

        return argumentValue.AnyLetters() && !bool.TryParse(argumentValue, out _);
    }

    public CliInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        return new ValuedCliInstructionArgument<string>(argumentName, validArgumentValue);
    }
}