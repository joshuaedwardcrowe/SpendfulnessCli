namespace YnabProgressConsole.Instructions.InstructionArguments;

public static class InstructionArgumentExtensions
{
    public static TypedInstructionArgument<TArgumentType>? OfType<TArgumentType>(
        this IEnumerable<InstructionArgument> arguments, string argumentName) where TArgumentType : notnull
        => arguments
            .Where(argument => argument.ArgumentName == argumentName)
            .OfType<TypedInstructionArgument<TArgumentType>>()
            .FirstOrDefault();
}