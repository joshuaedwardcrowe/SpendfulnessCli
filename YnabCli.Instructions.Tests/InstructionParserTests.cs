using NUnit.Framework;
using YnabCli.Instructions.Arguments;
using YnabCli.Instructions.Builders;
using YnabCli.Instructions.Extraction;
using YnabCli.Instructions.Indexers;
using YnabCli.Instructions.Parsers;

namespace YnabCli.Instructions.Tests;

[TestFixture]
public class InstructionParserTests
{
    private InstructionTokenIndexer _instructionTokenIndexer;
    private InstructionTokenExtractor _instructionTokenExtractor;
    private IEnumerable<IInstructionArgumentBuilder> _instructionArgumentBuilders;
    private InstructionParser _parser;

    [SetUp]
    public void SetUp()
    {
        _instructionTokenIndexer = new InstructionTokenIndexer();
        
        _instructionTokenExtractor = new InstructionTokenExtractor();
        
        _instructionArgumentBuilders = new List<IInstructionArgumentBuilder>
        {
            new StringInstructionArgumentBuilder(),
            new IntInstructionArgumentBuilder(),
        };

        _parser = new InstructionParser(
            _instructionTokenIndexer,
            _instructionTokenExtractor,
            _instructionArgumentBuilders);
    }

    [Test]
    public void GivenParserTokensWithPrefix_WhenParse_ThenReturnsInstructionWithPrefix()
    {
        var result = _parser.Parse("/name");
        
        Assert.That(result.Prefix, Is.EqualTo("/"));
    }

    [Test]
    public void GivenParserTokensWithName_WhenParse_ThenReturnsInstructionWithName()
    {
        var result = _parser.Parse("/name");
        
        Assert.That(result.Name, Is.EqualTo("name"));
    }

    [Test]
    public void GivenExtractionWithSubNae_WhenParse_ThenReturnsInstructionWithSubNae()
    {
        var result = _parser.Parse("/name subname");
        
        Assert.That(result.SubName, Is.EqualTo("subname"));
    }

    [Test]
    public void GivenParserWithStringArguments_WhenParse_ThenReturnsInstructionWithStringTypedArguments()
    {
        var result = _parser.Parse("/command --argument-one hello world");

        var argument = result.Arguments
            .OfType<TypedInstructionArgument<string>>()
            .FirstOrDefault();
        
        Assert.That(argument, Is.Not.Null);
    }
    
    
    [Test]
    public void GivenParserWithIntArguments_WhenParse_ThenReturnsInstructionWithIntTypedArguments()
    {
        var result = _parser.Parse("/name --argument-one 1");

        var argument = result.Arguments
            .OfType<TypedInstructionArgument<int>>()
            .FirstOrDefault();
        
        Assert.That(argument, Is.Not.Null);
    }
}