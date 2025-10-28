using Cli.Instructions.Abstractions;
using Cli.Instructions.Indexers;

namespace Cli.Instructions.Extraction;

public class CliInstructionTokenExtractor
{
    public CliInstructionTokenExtraction Extract(
        Dictionary<CliInstructionTokenType, CliInstructionTokenIndex> indexes, 
        string terminalInput)
    {
        var prefixToken = ExtractToken(
            indexes, 
            terminalInput, 
            CliInstructionTokenType.Prefix, 
            CliInstructionExceptionCode.NoInstructionPrefix,
            $"Instructions must contain a {CliInstructionConstants.DefaultNamePrefix}",
            isRequired: true);
        
        var nameToken = ExtractToken(
            indexes, 
            terminalInput, 
            CliInstructionTokenType.Name, 
            CliInstructionExceptionCode.NoInstructionName,
            "Instructions must have a name",
            isRequired: true);
        
        var subNameToken = ExtractToken(
            indexes, 
            terminalInput, 
            CliInstructionTokenType.SubName, 
            CliInstructionExceptionCode.NoInstructionName,
            null,
            isRequired: false);
        
        var argumentTokens = ExtractArgumentTokens(indexes, terminalInput);
        
        return new CliInstructionTokenExtraction(prefixToken!, nameToken!, subNameToken, argumentTokens);
    }

    private string? ExtractToken(
        Dictionary<CliInstructionTokenType, CliInstructionTokenIndex> indexes,
        string terminalInput,
        CliInstructionTokenType tokenType,
        CliInstructionExceptionCode exceptionCode,
        string? exceptionMessage,
        bool isRequired)
    {
        var tokenIndex = indexes[tokenType];
        
        if (!tokenIndex.Found)
        {
            if (isRequired)
            {
                throw new CliInstructionException(exceptionCode, exceptionMessage!);
            }
            return null;
        }

        return terminalInput[tokenIndex.StartIndex..tokenIndex.EndIndex];
    }
    
    private static Dictionary<string, string?> ExtractArgumentTokens(
        Dictionary<CliInstructionTokenType, CliInstructionTokenIndex> indexes, 
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