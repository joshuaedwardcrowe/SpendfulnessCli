namespace YnabCli.Instructions.Arguments;

public class TypedInstructionArgument<TArgumentValue>(string argumentName, TArgumentValue argumentValue)
    : InstructionArgument(argumentName) where TArgumentValue : notnull
{
    public TArgumentValue ArgumentValue { get; set; } = argumentValue;
}