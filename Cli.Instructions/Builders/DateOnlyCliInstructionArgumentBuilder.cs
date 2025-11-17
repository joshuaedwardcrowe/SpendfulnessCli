using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

// TODO: Write unit tests.
internal class DateOnlyCliInstructionArgumentBuilder : NoDefaultInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => DateTime.TryParse(argumentValue, out _);

    public CliInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var argumentDate = DateTime.Parse(validArgumentValue);
        var argumentDateOnly = DateOnly.FromDateTime(argumentDate);

        return new ValuedCliInstructionArgument<DateOnly>(argumentName, argumentDateOnly);
    }
}