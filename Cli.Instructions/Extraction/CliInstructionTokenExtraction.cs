namespace Cli.Instructions.Extraction;

public record CliInstructionTokenExtraction(
    string? PrefixToken,
    string? NameToken,
    string? SubNameToken,
    Dictionary<string, string?> ArgumentTokens);