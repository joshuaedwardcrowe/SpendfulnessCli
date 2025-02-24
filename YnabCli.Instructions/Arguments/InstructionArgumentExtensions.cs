namespace YnabCli.Instructions.Arguments;

public static class InstructionArgumentExtensions
{
    public static TypedInstructionArgument<TArgumentType>? OfType<TArgumentType>(
        this IEnumerable<InstructionArgument> arguments, string argumentName) where TArgumentType : notnull
            => arguments
                .Where(argument => argument.ArgumentName == argumentName)
                .OfType<TypedInstructionArgument<TArgumentType>>()
                .FirstOrDefault();

    public static TypedInstructionArgument<TArgumentType> OfRequiredType<TArgumentType>(
        this IEnumerable<InstructionArgument> arguments, string argumentName) where TArgumentType : notnull
    {
        var argument = OfType<TArgumentType>(arguments, argumentName);

        if (argument is null)
        {
            throw new ArgumentException($"Argument '{argumentName}' is required.");
        }
        
        return argument;
    }
}