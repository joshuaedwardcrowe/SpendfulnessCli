using YnabProgressConsole.Instructions.InstructionArgumentBuilders;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions;

public class InstructionParser
{
    private readonly IEnumerable<IInstructionArgumentBuilder> _instructionArgumentBuilders;
    private const string InstructionArgumentPrefix = "--";

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
        var includesInputBeyondCommandName = terminalInput.Contains(' ');
        
        var indexAfterCommandName = includesInputBeyondCommandName 
            ? terminalInput.IndexOf(' ') // At the end of the command name
            : terminalInput.Length - 1; // The end of the input will be the end of the command name/

        var indexOfCommandToken = terminalInput.IndexOf('/');

        var commandPrefixToken = indexOfCommandToken > 0
            ? terminalInput.Substring(0, 1)
            : null;
        
        var commandNameToken = terminalInput.Substring(1, indexAfterCommandName);
        
        var remainingTerminalInput = includesInputBeyondCommandName
            ? terminalInput.Substring(indexAfterCommandName + 1) 
            : string.Empty;
        
        var argumentTokens = remainingTerminalInput
            .Split(InstructionArgumentPrefix)
            .Where(token => token != string.Empty);

        return new InstructionTokens(commandPrefixToken, commandNameToken, argumentTokens);
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