using Cli.Instructions.Abstractions;
using Cli.Instructions.Builders;
using Cli.Instructions.Extraction;
using Cli.Instructions.Indexers;

namespace Cli.Instructions.Parsers;

// TODO: Rename to CliInstructionParser
public class CliInstructionParser
{
    private readonly CliInstructionTokenIndexer _cliInstructionTokenIndexer;
    private readonly CliInstructionTokenExtractor _cliInstructionTokenExtractor;
    private readonly IEnumerable<ICliInstructionArgumentBuilder> _instructionArgumentBuilders;

    public CliInstructionParser(
        CliInstructionTokenIndexer cliInstructionTokenIndexer,
        CliInstructionTokenExtractor cliInstructionTokenExtractor,
        IEnumerable<ICliInstructionArgumentBuilder> instructionArgumentBuilders)
    {
        _cliInstructionTokenIndexer = cliInstructionTokenIndexer;
        _cliInstructionTokenExtractor = cliInstructionTokenExtractor;
        _instructionArgumentBuilders = instructionArgumentBuilders;
    }

    public ConsoleInstruction Parse(string terminalInput)
    {
        var indexes = _cliInstructionTokenIndexer.Index(terminalInput);
        var extraction = _cliInstructionTokenExtractor.Extract(indexes, terminalInput);
        
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