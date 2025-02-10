using YnabProgressConsole.Instructions.InstructionArgumentBuilders;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions;

public class InstructionParser
{
    private readonly IEnumerable<IInstructionArgumentBuilder> _instructionArgumentBuilders;
    private const string DefaultNamePrefix = "/";
    private const string DefaultArgumentPrefix = "--";

    public InstructionParser(IEnumerable<IInstructionArgumentBuilder> instructionArgumentBuilders)
    {
        _instructionArgumentBuilders = instructionArgumentBuilders;
    }
    
    public Instruction Parse(string input)
    {
        var tokens = PerformTokenParse(input);
        var arguments = ParseInstructionArguments(tokens.ArgumentTokens);

        return new Instruction(tokens.PrefixToken, tokens.NameToken, arguments);
    }
    
    private InstructionTokens PerformTokenParse(string terminalInput)
    {
        var includesInputBeyondNameToken = terminalInput.Contains(' ');
        
        var indexAfterNameToken = includesInputBeyondNameToken 
            ? terminalInput.IndexOf(' ')  // At the end of the command name
            : terminalInput.Length; // The end of the input will be the end of the command name/
        
        var namePrefixToken = PerformNamePrefixTokeNParse(terminalInput);
        
        var nameToken = PerformNameTokenParse(terminalInput, indexAfterNameToken);
        
        var argumentTokens = PerformArgumentTokenParse(
            terminalInput, indexAfterNameToken, includesInputBeyondNameToken);

        return new InstructionTokens(namePrefixToken, nameToken, argumentTokens);
    }

    private string? PerformNamePrefixTokeNParse(string terminalInput)
    {
        var indexOfNamePrefixToken = terminalInput.IndexOf(DefaultNamePrefix, StringComparison.CurrentCulture);
        return indexOfNamePrefixToken == 0 ? terminalInput[..1] : null;
    }

    private string PerformNameTokenParse(string terminalInput, int indexAfterNameToken)
    {
        var indexOfEndOfNameToken = indexAfterNameToken - 1;
        return terminalInput.Substring(1, indexOfEndOfNameToken);
    }

    private IEnumerable<string> PerformArgumentTokenParse(
        string terminalInput, int indexAfterNameToken, bool includesInputBeyondNameToken)
    {
        var indexOfStartOfRemainingInput = indexAfterNameToken + 1;
        
        var remainingTerminalInput = includesInputBeyondNameToken
            ? terminalInput.Substring(indexOfStartOfRemainingInput)
            : string.Empty;
        
        return remainingTerminalInput
            .Split(DefaultArgumentPrefix)
            .Where(token => token != string.Empty);
    }

    private IEnumerable<InstructionArgument> ParseInstructionArguments(IEnumerable<string> argumentTokens)
    {
        foreach (var argumentToken in argumentTokens)
        {
            var trimmedArgumentToken = argumentToken.Trim();
            var firstSpaceIndex = trimmedArgumentToken.IndexOf(' ');
            
            var argumentName = trimmedArgumentToken.Substring(0, firstSpaceIndex);
            var argumentValue = trimmedArgumentToken.Substring(firstSpaceIndex + 1);
            
            var argument = _instructionArgumentBuilders
                .First(x => x.For(argumentValue))
                .Create(argumentName, argumentValue);
            
            yield return argument;
        }
    }
}