using Cli.Instructions.Builders;
using Cli.Instructions.Extraction;
using Cli.Instructions.Indexers;

namespace Cli.Instructions.Parsers;

// TODO: Rename to CliInstructionParser
public class ConsoleInstructionParser
{
    private readonly ConsoleInstructionTokenIndexer _consoleInstructionTokenIndexer;
    private readonly ConsoleInstructionTokenExtractor _consoleInstructionTokenExtractor;
    private readonly IEnumerable<IConsoleInstructionArgumentBuilder> _instructionArgumentBuilders;

    public ConsoleInstructionParser(
        ConsoleInstructionTokenIndexer consoleInstructionTokenIndexer,
        ConsoleInstructionTokenExtractor consoleInstructionTokenExtractor,
        IEnumerable<IConsoleInstructionArgumentBuilder> instructionArgumentBuilders)
    {
        _consoleInstructionTokenIndexer = consoleInstructionTokenIndexer;
        _consoleInstructionTokenExtractor = consoleInstructionTokenExtractor;
        _instructionArgumentBuilders = instructionArgumentBuilders;
    }

    public ConsoleInstruction Parse(string terminalInput)
    {
        var indexes = _consoleInstructionTokenIndexer.Index(terminalInput);
        var extraction = _consoleInstructionTokenExtractor.Extract(indexes, terminalInput);
        
        var arguments = extraction
            .ArgumentTokens
            .Select(token => _instructionArgumentBuilders
                .First(builder => builder.For(token.Value))
                .Create(token.Key, token.Value));
        
        return new ConsoleInstruction(
            extraction.PrefixToken,
            extraction.NameToken,
            extraction.SubNameToken,
            arguments);
    }
}