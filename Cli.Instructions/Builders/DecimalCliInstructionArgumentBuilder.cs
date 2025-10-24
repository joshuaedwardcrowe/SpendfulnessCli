using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

public class DecimalCliInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => decimal.TryParse(argumentValue, out _);

    public ConsoleInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var parsedArgumentValue = decimal.Parse(validArgumentValue);
        return new TypedConsoleInstructionArgument<decimal>(argumentName, parsedArgumentValue);
    }
}