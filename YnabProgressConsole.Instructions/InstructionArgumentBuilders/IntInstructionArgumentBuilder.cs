using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions.InstructionArgumentBuilders;

public class IntInstructionArgumentBuilder : IInstructionArgumentBuilder
{
    public bool For(string argumentValue)
        => argumentValue.ToCharArray().All(char.IsNumber);

    public InstructionArgument Create(string argumentName, string argumentValue)
    {
        if (!int.TryParse(argumentValue, out var convertedArgumentValue))
        {
            throw new ArgumentException($"Invalid Integer Value for Argument: {argumentName}");
        }
        
        return new TypedInstructionArgument<int>(argumentName, convertedArgumentValue);
    }
}