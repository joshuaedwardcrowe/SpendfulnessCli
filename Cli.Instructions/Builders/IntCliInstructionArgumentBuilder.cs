using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

public class IntCliInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => argumentValue != null && int.TryParse(argumentValue, out _);

    public ConsoleInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var parsedArgumentValue = int.Parse(validArgumentValue);
        return new TypedConsoleInstructionArgument<int>(argumentName, parsedArgumentValue);
    }
}