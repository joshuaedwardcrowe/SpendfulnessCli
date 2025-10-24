using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Indexers;

public class CliInstructionTokenIndexer
{
    public ConsoleInstructionTokenIndexes Index(string terminalInput)
    {
        var characters = terminalInput.ToCharArray();
        if (characters.Length == 0)
        {
            return new ConsoleInstructionTokenIndexes
            {
                PrefixTokenIndexed = false,
                NameTokenIndexed = false,
                SubNameTokenIndexed = false,
                ArgumentTokensIndexed = false,
            };
        }

        // Constraint: Command must be prefixed with some kind of punctuation marl
        // Even though the current default is '/'
        var firstPunctuationMark = characters.FirstOrDefault(char.IsPunctuation);
        
        // e.g. <here>/spare-money help --argumentOne hello world --argumentTwo 1
        var firstPunctuationMarkIndex = terminalInput.IndexOf(firstPunctuationMark);
        var hasFirstPunctuationMark = firstPunctuationMarkIndex == 0;
        
        // e.g. /<here>spare-money help --argumentOne hello world --argumentTwo 1
        var afterPunctuationMarkIndex = firstPunctuationMarkIndex + 1;
 
        var firstSpaceIndex = terminalInput.IndexOf(ConsoleInstructionConstants.DefaultSpaceCharacter);
        var hasFirstSpace = firstSpaceIndex != -1;
        
        // If command name is present, the first space should not be after the /
        var hasCommandNameToken = firstSpaceIndex != afterPunctuationMarkIndex;
        
        // e.g. /spare-money<here> help --argumentOne hello world --argumentTwo 1
        var commandNameTokenEndIndex = hasFirstSpace ? firstSpaceIndex : terminalInput.Length;
        
        // e.g. /spare-money help <here>--argumentOne hello world --argumentTwo 1
        var firstArgumentIndex = terminalInput.IndexOf(
            ConsoleInstructionConstants.DefaultArgumentPrefix,
            StringComparison.Ordinal);
        
        var hasArgumentTokens = firstArgumentIndex != -1;

        var beforeFirstArgumentIndex = firstArgumentIndex - 1;
        var inputBetweenFirstSpaceAndFirstArgument = beforeFirstArgumentIndex != firstSpaceIndex;
        
        // The space between the first argument and command name has nothing in it - if there is a space at all
        var hasSubCommandNameToken = hasFirstSpace && inputBetweenFirstSpaceAndFirstArgument;
        
        // e.g. /spare-money <here>help --argumentOne hello world --argumentTwo 1
        var subCommandNameStartIndex = firstSpaceIndex + 1;
        
        // e.g. /spare-money help<here> --argumentOne hello world --argumentTwo 1
        var subCommandNameEndIndex = hasArgumentTokens ? firstArgumentIndex - 1 : terminalInput.Length;
        
        return new ConsoleInstructionTokenIndexes
        {
            PrefixTokenIndexed = hasFirstPunctuationMark,
            PrefixTokenStartIndex = firstPunctuationMarkIndex,
            PrefixTokenEndIndex = afterPunctuationMarkIndex,
            
            NameTokenIndexed = hasCommandNameToken,
            NameTokenStartIndex = afterPunctuationMarkIndex,
            NameTokenEndIndex = commandNameTokenEndIndex,
            
            SubNameTokenIndexed = hasSubCommandNameToken,
            SubNameStartIndex = subCommandNameStartIndex,
            SubNameEndIndex = subCommandNameEndIndex,
            
            ArgumentTokensIndexed = hasArgumentTokens,
            ArgumentTokensStartIndex = firstArgumentIndex,
            ArgumentTokensEndIndex = terminalInput.Length
        };
    }
}