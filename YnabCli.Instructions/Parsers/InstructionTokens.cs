namespace YnabCli.Instructions.Parsers;

public record InstructionTokens(
    string? PrefixToken,
    string? NameToken,
    string? SubNameToken,
    Dictionary<string, string?>? ArgumentTokens);