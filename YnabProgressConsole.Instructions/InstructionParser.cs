using YnabProgressConsole.Instructions.InstructionArgumentBuilders;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions;

public class InstructionParser
{
    private readonly IEnumerable<IInstructionArgumentBuilder> _instructionArgumentBuilders;
    private const string DefaultNamePrefix = "-/";
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

        var indexOfNamePrefixToken = terminalInput.IndexOf(DefaultNamePrefix, StringComparison.CurrentCulture);

        var namePrefixToken = indexOfNamePrefixToken == 0
            ? terminalInput.Substring(0, 1)
            : null;
        
        var nameToken = terminalInput.Substring(1, indexAfterNameToken - 1);
        
        var remainingTerminalInput = includesInputBeyondNameToken
            ? terminalInput.Substring(indexAfterNameToken + 1)
            : string.Empty;
        
        var argumentTokens = remainingTerminalInput
            .Split(DefaultArgumentPrefix)
            .Where(token => token != string.Empty);

        return new InstructionTokens(namePrefixToken, nameToken, argumentTokens);
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