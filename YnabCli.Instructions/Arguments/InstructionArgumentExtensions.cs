using YnabCli.Instructions.Exceptions;

namespace YnabCli.Instructions.Arguments;

public static class InstructionArgumentExtensions
{
    public static TypedInstructionArgument<TArgumentType>? OfType<TArgumentType>(
        this IEnumerable<InstructionArgument> arguments, string argumentName)
        where TArgumentType : notnull
            => arguments
                .Where(argument => argument.ArgumentName == argumentName)
                .OfType<TypedInstructionArgument<TArgumentType>>()
                .FirstOrDefault();

    // TODO: Im not sure this returning the type is even worth it? Just return the value?
    public static TypedInstructionArgument<TArgumentType> OfRequiredType<TArgumentType>(
        this IEnumerable<InstructionArgument> arguments, string argumentName)
        where TArgumentType : notnull
    {
        var argument = OfType<TArgumentType>(arguments, argumentName);

        if (argument is null)
        {
            throw new InstructionException(InstructionExceptionCode.ArgumentIsRequired,
                $"Argument '{argumentName}' is required for this command.");
        }
        
        return argument;
    }

    public static TypedInstructionArgument<string> OfRequiredStringFrom<TPossibleArgumentType>(
        this IEnumerable<InstructionArgument> arguments, string argumentName)
        where TPossibleArgumentType : notnull
    {
        var possibleArgument = OfRequiredType<TPossibleArgumentType>(arguments, argumentName);
        var stringifiedArgumentValue = possibleArgument.ArgumentValue.ToString();
        return new TypedInstructionArgument<string>(argumentName, stringifiedArgumentValue!);
    }

    // Works around the constraint that argument passing has to be one or the other, but we can 
    // work out which one it is.
    public static TypedInstructionArgument<string> OfRequiredStringFrom<TPossibleArgumentTypeOne, TPossibleArgumentTypeTwo>(
        this List<InstructionArgument> arguments, string argumentName) 
        where TPossibleArgumentTypeOne : notnull
        where TPossibleArgumentTypeTwo : notnull
    {
        var possibleArgumentOne = OfType<TPossibleArgumentTypeOne>(arguments, argumentName);
        if (possibleArgumentOne is not null)
        {
            var stringifiedArgumentValue = possibleArgumentOne.ArgumentValue.ToString();
            return new TypedInstructionArgument<string>(argumentName, stringifiedArgumentValue!);
        }
        
        var possibleArgumentTwo = OfType<TPossibleArgumentTypeTwo>(arguments, argumentName);
        if (possibleArgumentTwo is not null)
        {
            var stringifiedArgumentValue = possibleArgumentTwo.ArgumentValue.ToString();
            return new TypedInstructionArgument<string>(argumentName, stringifiedArgumentValue!);
        }
        
        throw new InstructionException(InstructionExceptionCode.ArgumentIsRequired,
                $"Argument '{argumentName}' is required for this command.");
    }
}