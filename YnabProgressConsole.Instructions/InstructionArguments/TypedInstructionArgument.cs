namespace YnabProgressConsole.Instructions.InstructionArguments;

public class TypedInstructionArgument<TEvaluation>(string argumentName, TEvaluation argumentValue)
    : InstructionArgument(argumentName) where TEvaluation : notnull
{
    public  TEvaluation ArgumentValue { get; set; } = argumentValue;
}