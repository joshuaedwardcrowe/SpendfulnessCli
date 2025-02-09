using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions;

public record Instruction(
    string InstructionPrefix,
    string InstructionName,
    IEnumerable<InstructionArgument> Arguments);