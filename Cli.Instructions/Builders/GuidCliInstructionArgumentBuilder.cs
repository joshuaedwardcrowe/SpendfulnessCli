using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

public class GuidCliInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => Guid.TryParse(argumentValue, out _);

    public ConsoleInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var parsedArgumentValue = Guid.Parse(validArgumentValue);
        return new TypedConsoleInstructionArgument<Guid>(argumentName, parsedArgumentValue);
    }
}