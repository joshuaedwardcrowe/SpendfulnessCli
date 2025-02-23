namespace YnabCli.Instructions.Parsers;

public record LegacyInstructionTokens(
    string? CommandPrefixToken,
    string CommandNameToken,
    Dictionary<string, string?> ArgumentTokens);