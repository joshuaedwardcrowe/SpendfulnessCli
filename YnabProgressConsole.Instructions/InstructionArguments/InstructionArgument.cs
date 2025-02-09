namespace YnabProgressConsole.Instructions.InstructionArguments;

public class InstructionArgument(string argumentName)
{
    public string ArgumentName { get; set; } = argumentName;
}