using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

internal class DecimalCliInstructionArgumentBuilder : CliInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => decimal.TryParse(argumentValue, out _);

    public CliInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var parsedArgumentValue = decimal.Parse(validArgumentValue);
        return new ValuedCliInstructionArgument<decimal>(argumentName, parsedArgumentValue);
    }
}