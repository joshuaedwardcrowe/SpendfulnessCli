using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Ynab.Commands.Reporting;

public static class InstructionArgumentExtensions
{
    public static TypedConsoleInstructionArgument<decimal>? OfCurrencyType(
        this List<ConsoleInstructionArgument> arguments, string argumentName)
    {
        var minusIntArgument = arguments.OfType<int>(argumentName);
        if (minusIntArgument == null)
        {
            return arguments.OfType<decimal>(argumentName);
        }

        return new TypedConsoleInstructionArgument<decimal>(
            minusIntArgument.ArgumentName,
            minusIntArgument.ArgumentValue);
    }
}