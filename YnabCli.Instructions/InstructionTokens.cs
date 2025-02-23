namespace YnabCli.Instructions;

public record InstructionTokens(
    string? CommandPrefixToken,
    string CommandNameToken,
    Dictionary<string, string?> ArgumentTokens);