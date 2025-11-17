using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting;

public static class InstructionArgumentExtensions
{
    public static ValuedCliInstructionArgument<decimal>? OfCurrencyType(
        this List<CliInstructionArgument> arguments, string argumentName)
    {
        var minusIntArgument = arguments.OfType<int>(argumentName);
        if (minusIntArgument == null)
        {
            return arguments.OfType<decimal>(argumentName);
        }

        return new ValuedCliInstructionArgument<decimal>(
            minusIntArgument.ArgumentName,
            minusIntArgument.ArgumentValue);
    }
}