using YnabCli.Instructions.Indexers;

namespace YnabCli.Instructions.Parsers;

public class InstructionTokenParser
{
    private readonly InstructionTokenIndexer _instructionTokenIndexer;

    public InstructionTokenParser(InstructionTokenIndexer instructionTokenIndexer)
    {
        _instructionTokenIndexer = instructionTokenIndexer;
    }

    public InstructionTokens Parse(string terminalInput)
    {
        var indexes = _instructionTokenIndexer.Index(terminalInput);
        
        var prefixToken = indexes.PrefixTokenIndexed 
            ? terminalInput[indexes.PrefixTokenStartIndex..indexes.PrefixTokenEndIndex]
            : null;

        var nameToken = indexes.NameTokenIndexed
            ? terminalInput[indexes.NameTokenStartIndex..indexes.NameTokenEndIndex]
            : null;

        var subNameToken = indexes.SubNameTokenIndexed
            ? terminalInput[indexes.SubNameStartIndex..indexes.SubNameEndIndex]
            : null;

        var argumentTokens = indexes.ArgumentTokensIndexed
            ? GetArgumentTokens(indexes, terminalInput)
            : null;
        
        return new InstructionTokens(prefixToken, nameToken, subNameToken, argumentTokens);
    }
    
    private static Dictionary<string, string?> GetArgumentTokens(InstructionTokenIndexes indexes, string terminalInput)
    {
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