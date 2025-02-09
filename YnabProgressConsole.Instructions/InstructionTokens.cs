namespace YnabProgressConsole.Instructions;

public record InstructionTokens(
    string? PrefixToken,
    string NameToken,
    IEnumerable<string> ArgumentTokens);