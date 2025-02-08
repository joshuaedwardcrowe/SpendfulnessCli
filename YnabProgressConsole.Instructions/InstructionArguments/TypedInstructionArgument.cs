namespace YnabProgressConsole.Instructions.InstructionArguments;

public class TypedInstructionArgument<TEvaluation> : InstructionArgument where TEvaluation : notnull
{
    public required TEvaluation ArgumentValue { get; set; }
}