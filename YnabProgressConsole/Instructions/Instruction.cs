using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions;

public class Instruction
{
    public required string InstructionName { get; set; }
    public required IEnumerable<InstructionArgument> Arguments { get; set; }
}