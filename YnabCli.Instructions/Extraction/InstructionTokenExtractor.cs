using YnabCli.Instructions.Indexers;

namespace YnabCli.Instructions.Extraction;

public class InstructionTokenExtractor
{
    public InstructionTokenExtraction Extract(InstructionTokenIndexes indexes, string terminalInput)
    {
        var prefixToken = ExtractPrefixToken(indexes, terminalInput);
        var nameToken = ExtractNameToken(indexes, terminalInput);
        var subNameToken = ExtractSubNameToken(indexes, terminalInput);
        var argumentTokens = ExtractArgumentTokens(indexes, terminalInput);
        
        return new InstructionTokenExtraction(prefixToken, nameToken, subNameToken, argumentTokens);
    }

    private string ExtractPrefixToken(InstructionTokenIndexes indexes, string terminalInput)
    {
        if (!indexes.PrefixTokenIndexed)
        {
            throw new ArgumentNullException($"Commands must contain a {InstructionConstants.DefaultNamePrefix}");
        }

        return terminalInput[indexes.PrefixTokenStartIndex..indexes.PrefixTokenEndIndex];
    }

    private string ExtractNameToken(InstructionTokenIndexes indexes, string terminalInput)
    {
        if (!indexes.NameTokenIndexed)
        {
            throw new ArgumentNullException($"Commands must have a name");
        }

        return terminalInput[indexes.NameTokenStartIndex..indexes.NameTokenEndIndex];
    }

    private string? ExtractSubNameToken(InstructionTokenIndexes indexes, string terminalInput)
    {
        if (!indexes.SubNameTokenIndexed)
        {
            return null;
        }

        return terminalInput[indexes.SubNameStartIndex..indexes.SubNameEndIndex];
    }
    
    private static Dictionary<string, string?> ExtractArgumentTokens(InstructionTokenIndexes indexes, string terminalInput)
    {
        if (!indexes.ArgumentTokensIndexed)
        {
            return new Dictionary<string, string?>();
        }
        
        var argumentInput = terminalInput[indexes.ArgumentTokensStartIndex..indexes.ArgumentTokensEndIndex];

        var argumentTokens = argumentInput.Split(InstructionConstants.DefaultArgumentPrefix);
        
        var validArgumentTokens = argumentTokens.Where(i => !string.IsNullOrWhiteSpace(i));
        
        var parsedArgumentTokens = validArgumentTokens.Select(ParseArgumentInput);
        
        return parsedArgumentTokens.ToDictionary(token => token.Key, token => token.Value);
    }
    
    private static KeyValuePair<string, string?> ParseArgumentInput(string terminalArgumentInput)
    {
        // e.g. --payee-name Subway Something Something
        var firstIndexOfSpace = terminalArgumentInput.IndexOf(InstructionConstants.DefaultSpaceCharacter);
        
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