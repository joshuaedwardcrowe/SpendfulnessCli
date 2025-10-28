using Cli.Instructions.Abstractions;
using Cli.Instructions.Extensions;
using Cli.Instructions.Indexers;

namespace Cli.Instructions.Extraction;

public class CliInstructionTokenExtractor
{
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
            ThrowMissingTokenException(tokenType);
        }

        return terminalInput.ExtractTokenContent(tokenIndex);
    }

    private static void ThrowMissingTokenException(CliInstructionTokenType tokenType)
    {
        switch (tokenType)
        {
            case CliInstructionTokenType.Prefix:
                throw new NoInstructionPrefixException(
                    $"Instructions must contain a {CliInstructionConstants.DefaultNamePrefix}");
            case CliInstructionTokenType.Name:
                throw new NoInstructionNameException("Instructions must have a name");
            default:
                throw new CliInstructionException(
                    CliInstructionExceptionCode.NoInstructionName,
                    $"Missing required token: {tokenType}");
        }
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

        return terminalInput.ExtractTokenContent(tokenIndex);
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
        
        var argumentInput = terminalInput.ExtractTokenContent(argumentIndex);

        return argumentInput
            .Split(CliInstructionConstants.DefaultArgumentPrefix)
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .Select(i => i.Trim())
            .Select(ParseArgumentInput)
            .ToDictionary(token => token.Key, token => token.Value);
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