using YnabCli.Instructions.InstructionArgumentBuilders;
using YnabCli.Instructions.InstructionArguments;

namespace YnabCli.Instructions;

public class InstructionParser(IEnumerable<IInstructionArgumentBuilder> instructionArgumentBuilders)
{
    public Instruction Parse(InstructionTokens tokens)
    {
        var arguments = MapInstructionArguments(tokens.ArgumentTokens);

        return new Instruction(tokens.CommandPrefixToken, tokens.CommandNameToken, arguments);
    }

    private IEnumerable<InstructionArgument> MapInstructionArguments(Dictionary<string, string?> argumentTokens)
    {
        foreach (var argumentToken in argumentTokens)
        {
            var argument = instructionArgumentBuilders
                .First(x => x.For(argumentToken.Value))
                .Create(argumentToken.Key, argumentToken.Value);
            
            yield return argument;
        }
    }
}