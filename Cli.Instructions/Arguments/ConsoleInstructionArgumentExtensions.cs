using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Arguments;

public static class ConsoleInstructionArgumentExtensions
{
    public static TypedConsoleInstructionArgument<TArgumentType>? OfType<TArgumentType>(
        this IEnumerable<ConsoleInstructionArgument> arguments, string argumentName)
        where TArgumentType : notnull
            => arguments
                .Where(argument => argument.ArgumentName == argumentName)
                .OfType<TypedConsoleInstructionArgument<TArgumentType>>()
                .FirstOrDefault();

    // TODO: Im not sure this returning the type is even worth it? Just return the value?
    public static TypedConsoleInstructionArgument<TArgumentType> OfRequiredType<TArgumentType>(
        this IEnumerable<ConsoleInstructionArgument> arguments, string argumentName)
        where TArgumentType : notnull
    {
        var argument = OfType<TArgumentType>(arguments, argumentName);

        if (argument is null)
        {
            throw new ConsoleInstructionException(ConsoleInstructionExceptionCode.ArgumentIsRequired,
                $"Argument '{argumentName}' is required for this command.");
        }
        
        return argument;
    }

    public static TypedConsoleInstructionArgument<string> OfRequiredStringFrom<TPossibleArgumentType>(
        this IEnumerable<ConsoleInstructionArgument> arguments, string argumentName)
        where TPossibleArgumentType : notnull
    {
        var possibleArgument = OfRequiredType<TPossibleArgumentType>(arguments, argumentName);
        var stringifiedArgumentValue = possibleArgument.ArgumentValue.ToString();
        return new TypedConsoleInstructionArgument<string>(argumentName, stringifiedArgumentValue!);
    }

    // Works around the constraint that argument passing has to be one or the other, but we can 
    // work out which one it is.
    public static TypedConsoleInstructionArgument<string> OfRequiredStringFrom<TPossibleArgumentTypeOne, TPossibleArgumentTypeTwo>(
        this List<ConsoleInstructionArgument> arguments, string argumentName) 
        where TPossibleArgumentTypeOne : notnull
        where TPossibleArgumentTypeTwo : notnull
    {
        var possibleArgumentOne = OfType<TPossibleArgumentTypeOne>(arguments, argumentName);
        if (possibleArgumentOne is not null)
        {
            var stringifiedArgumentValue = possibleArgumentOne.ArgumentValue.ToString();
            return new TypedConsoleInstructionArgument<string>(argumentName, stringifiedArgumentValue!);
        }
        
        var possibleArgumentTwo = OfType<TPossibleArgumentTypeTwo>(arguments, argumentName);
        if (possibleArgumentTwo is not null)
        {
            var stringifiedArgumentValue = possibleArgumentTwo.ArgumentValue.ToString();
            return new TypedConsoleInstructionArgument<string>(argumentName, stringifiedArgumentValue!);
        }
        
        throw new ConsoleInstructionException(ConsoleInstructionExceptionCode.ArgumentIsRequired,
                $"Argument '{argumentName}' is required for this command.");
    }
}