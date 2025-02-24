using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Ynab;

public static class InstructionArgumentExtensions
{
    public static TypedInstructionArgument<decimal>? OfCurrencyType(
        this List<InstructionArgument> arguments, string argumentName)
    {
        var minusIntArgument = arguments.OfType<int>(argumentName);
        if (minusIntArgument == null)
        {
            return arguments.OfType<decimal>(argumentName);
        }

        return new TypedInstructionArgument<decimal>(
            minusIntArgument.ArgumentName,
            minusIntArgument.ArgumentValue);
    }
}