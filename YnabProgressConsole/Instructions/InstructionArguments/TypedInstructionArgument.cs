namespace YnabProgressConsole.Instructions.InstructionArguments;

public class TypedInstructionArgument<TEvaluation> : InstructionArgument
{
    public required TEvaluation ArgumentValue { get; set; }
}