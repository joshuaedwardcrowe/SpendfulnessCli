using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions;

public record Instruction(
    string? Prefix,
    string Name,
    IEnumerable<InstructionArgument> Arguments);