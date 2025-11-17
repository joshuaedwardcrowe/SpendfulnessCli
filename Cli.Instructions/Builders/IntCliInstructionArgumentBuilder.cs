using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

// TODO: Write unit tests.
internal class IntCliInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => argumentValue != null && int.TryParse(argumentValue, out _);

    public CliInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var parsedArgumentValue = int.Parse(validArgumentValue);
        return new ValuedCliInstructionArgument<int>(argumentName, parsedArgumentValue);
    }
}