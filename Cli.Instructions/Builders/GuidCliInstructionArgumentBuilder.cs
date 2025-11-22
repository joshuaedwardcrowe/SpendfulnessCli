using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Instructions.Builders;

internal class GuidCliInstructionArgumentBuilder : CliInstructionArgumentBuilder, ICliInstructionArgumentBuilder
{
    public bool For(string? argumentValue) => Guid.TryParse(argumentValue, out _);

    public CliInstructionArgument Create(string argumentName, string? argumentValue)
    {
        var validArgumentValue = GetValidValue(argumentName, argumentValue);
        var parsedArgumentValue = Guid.Parse(validArgumentValue);
        return new ValuedCliInstructionArgument<Guid>(argumentName, parsedArgumentValue);
    }
}