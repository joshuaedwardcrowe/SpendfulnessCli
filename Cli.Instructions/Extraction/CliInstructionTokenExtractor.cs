using Cli.Instructions.Abstractions;
using Cli.Instructions.Indexers;

namespace Cli.Instructions.Extraction;

public class CliInstructionTokenExtractor
{
    private static readonly Dictionary<CliInstructionTokenType, (CliInstructionExceptionCode Code, string Message)> RequiredTokenExceptions = new()
    {
        [CliInstructionTokenType.Prefix] = (
            CliInstructionExceptionCode.NoInstructionPrefix,
            $"Instructions must contain a {CliInstructionConstants.DefaultNamePrefix}"),
        [CliInstructionTokenType.Name] = (
            CliInstructionExceptionCode.NoInstructionName,
            "Instructions must have a name")
    };

    public CliInstructionTokenExtraction Extract(
        CliInstructionTokenIndexCollection indexes, 
        string terminalInput)
    {
        var prefixToken = ExtractRequiredToken(indexes, terminalInput, CliInstructionTokenType.Prefix);
        var nameToken = ExtractRequiredToken(indexes, terminalInput, CliInstructionTokenType.Name);
        var subNameToken = ExtractOptionalToken(indexes, terminalInput, CliInstructionTokenType.SubName);
        var argumentTokens = ExtractArgumentTokens(indexes, terminalInput);
        
        return new CliInstructionTokenExtraction(prefixToken, nameToken, subNameToken, argumentTokens);
    }

    private string ExtractRequiredToken(
        CliInstructionTokenIndexCollection indexes,
        string terminalInput,
        CliInstructionTokenType tokenType)
    {
        var tokenIndex = indexes[tokenType];
        
        if (!tokenIndex.Found)
        {
            var exception = RequiredTokenExceptions[tokenType];
            throw new CliInstructionException(exception.Code, exception.Message);
        }

        return ExtractTokenContent(terminalInput, tokenIndex);
    }

    private string? ExtractOptionalToken(
        CliInstructionTokenIndexCollection indexes,
        string terminalInput,
        CliInstructionTokenType tokenType)
    {
        var tokenIndex = indexes[tokenType];
        
        if (!tokenIndex.Found)
        {
            return null;
        }

        return ExtractTokenContent(terminalInput, tokenIndex);
    }

    private static string ExtractTokenContent(string terminalInput, CliInstructionTokenIndex tokenIndex)
    {
        return terminalInput[tokenIndex.StartIndex..tokenIndex.EndIndex];
    }
    
    private static Dictionary<string, string?> ExtractArgumentTokens(
        CliInstructionTokenIndexCollection indexes, 
        string terminalInput)
    {
        var argumentIndex = indexes[CliInstructionTokenType.Arguments];
        
        if (!argumentIndex.Found)
        {
            return new Dictionary<string, string?>();
        }
        
        var argumentInput = ExtractTokenContent(terminalInput, argumentIndex);

        var argumentTokens = argumentInput.Split(CliInstructionConstants.DefaultArgumentPrefix);
        
        var validArgumentTokens = argumentTokens
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .Select(i => i.Trim());
        
        var parsedArgumentTokens = validArgumentTokens.Select(ParseArgumentInput);
        
        return parsedArgumentTokens.ToDictionary(token => token.Key, token => token.Value);
    }
    
    private static KeyValuePair<string, string?> ParseArgumentInput(string terminalArgumentInput)
    {
        // e.g. --payee-name Subway Something Something
        var firstIndexOfSpace = terminalArgumentInput.IndexOf(CliInstructionConstants.DefaultSpaceCharacter);
        
        var argumentNameEndIndex = firstIndexOfSpace == -1
            ? terminalArgumentInput.Length
            : firstIndexOfSpace;

        var argumentName = terminalArgumentInput.Substring(0, argumentNameEndIndex);

        var argumentValue = firstIndexOfSpace == -1
            ? null
            : terminalArgumentInput[argumentNameEndIndex..].Trim();
        
        return new KeyValuePair<string, string?>(argumentName, argumentValue);
    }
}