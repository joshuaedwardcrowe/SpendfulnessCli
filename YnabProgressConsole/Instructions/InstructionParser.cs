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
        var name = ParseInstructionName(tokens.NameToken);
        var arguments = ParseInstructionArguments(tokens.ArgumentTokens);

        return new Instruction
        {
            InstructionName = name,
            Arguments = arguments,
        };
    }
    
    private InstructionTokens PerformTokenParse(string input)
    {
        var firstSpaceIndex = input.IndexOf(' ');
        
        var nameToken = input.Substring(0, firstSpaceIndex);
        
        var remainingInput = input.Substring(firstSpaceIndex + 1);
        
        var argumentTokens = remainingInput
            .Split(InstructionArgumentPrefix)
            .Where(token => token != string.Empty);

        return new InstructionTokens
        {
            NameToken = nameToken,
            ArgumentTokens = argumentTokens
        };
    }
    
    private string ParseInstructionName(string instructionNameToken) => instructionNameToken.Substring(1);
    
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