using Cli.Instructions.Abstractions;
using Cli.Instructions.Indexers;

namespace Cli.Instructions.Extraction;

public class CliInstructionTokenExtractor
{
    public CliInstructionTokenExtraction Extract(
        CliInstructionTokenIndexCollection indexes, 
        string terminalInput)
    {
        var prefixToken = ExtractRequiredToken(
            indexes, 
            terminalInput, 
            CliInstructionTokenType.Prefix, 
            CliInstructionExceptionCode.NoInstructionPrefix,
            $"Instructions must contain a {CliInstructionConstants.DefaultNamePrefix}");
        
        var nameToken = ExtractRequiredToken(
            indexes, 
            terminalInput, 
            CliInstructionTokenType.Name, 
            CliInstructionExceptionCode.NoInstructionName,
            "Instructions must have a name");
        
        var subNameToken = ExtractOptionalToken(
            indexes, 
            terminalInput, 
            CliInstructionTokenType.SubName);
        
        var argumentTokens = ExtractArgumentTokens(indexes, terminalInput);
        
        return new CliInstructionTokenExtraction(prefixToken, nameToken, subNameToken, argumentTokens);
    }

    private string ExtractRequiredToken(
        CliInstructionTokenIndexCollection indexes,
        string terminalInput,
        CliInstructionTokenType tokenType,
        CliInstructionExceptionCode exceptionCode,
        string exceptionMessage)
    {
        var tokenIndex = indexes[tokenType];
        
        if (!tokenIndex.Found)
        {
            throw new CliInstructionException(exceptionCode, exceptionMessage);
        }

        return terminalInput[tokenIndex.StartIndex..tokenIndex.EndIndex];
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
        
        var argumentInput = terminalInput[argumentIndex.StartIndex..argumentIndex.EndIndex];

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