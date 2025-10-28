using Cli.Instructions.Abstractions;

namespace Cli.Instructions.Indexers;

public class CliInstructionTokenIndexer
{
    // TODO: Make the default characters below configurable.
    // (SO this needs to be a DI service)
    public Dictionary<CliInstructionTokenType, CliInstructionTokenIndex> Index(string terminalInput)
    {
        var characters = terminalInput.ToCharArray();
        if (characters.Length == 0)
        {
            return new Dictionary<CliInstructionTokenType, CliInstructionTokenIndex>
            {
                [CliInstructionTokenType.Prefix] = new CliInstructionTokenIndex { Found = false },
                [CliInstructionTokenType.Name] = new CliInstructionTokenIndex { Found = false },
                [CliInstructionTokenType.SubName] = new CliInstructionTokenIndex { Found = false },
                [CliInstructionTokenType.Arguments] = new CliInstructionTokenIndex { Found = false }
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
 
        var firstSpaceIndex = terminalInput.IndexOf(CliInstructionConstants.DefaultSpaceCharacter);
        var hasFirstSpace = firstSpaceIndex != -1;
        
        // If command name is present, the first space should not be after the /
        var hasCommandNameToken = firstSpaceIndex != afterPunctuationMarkIndex;
        
        // e.g. /spare-money<here> help --argumentOne hello world --argumentTwo 1
        var commandNameTokenEndIndex = hasFirstSpace ? firstSpaceIndex : terminalInput.Length;
        
        // e.g. /spare-money help <here>--argumentOne hello world --argumentTwo 1
        var firstArgumentIndex = terminalInput.IndexOf(
            CliInstructionConstants.DefaultArgumentPrefix,
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
        
        return new Dictionary<CliInstructionTokenType, CliInstructionTokenIndex>
        {
            [CliInstructionTokenType.Prefix] = new CliInstructionTokenIndex
            {
                Found = hasFirstPunctuationMark,
                StartIndex = firstPunctuationMarkIndex,
                EndIndex = afterPunctuationMarkIndex
            },
            [CliInstructionTokenType.Name] = new CliInstructionTokenIndex
            {
                Found = hasCommandNameToken,
                StartIndex = afterPunctuationMarkIndex,
                EndIndex = commandNameTokenEndIndex
            },
            [CliInstructionTokenType.SubName] = new CliInstructionTokenIndex
            {
                Found = hasSubCommandNameToken,
                StartIndex = subCommandNameStartIndex,
                EndIndex = subCommandNameEndIndex
            },
            [CliInstructionTokenType.Arguments] = new CliInstructionTokenIndex
            {
                Found = hasArgumentTokens,
                StartIndex = firstArgumentIndex,
                EndIndex = terminalInput.Length
            }
        };
    }
}