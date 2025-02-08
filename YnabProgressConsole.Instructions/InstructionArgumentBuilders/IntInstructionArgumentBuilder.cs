using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions.InstructionArgumentBuilders;

public class IntInstructionArgumentBuilder : IInstructionArgumentBuilder
{
    public bool For(string argumentValue)
        => argumentValue.ToCharArray().All(char.IsNumber);

    public InstructionArgument Create(string argumentName, string argumentValue)
    {
        var convertedArgumentValue = int.Parse(argumentValue);
        
        return new TypedInstructionArgument<int>
        {
            ArgumentName = argumentName,
            ArgumentValue = convertedArgumentValue
        };
    }
}