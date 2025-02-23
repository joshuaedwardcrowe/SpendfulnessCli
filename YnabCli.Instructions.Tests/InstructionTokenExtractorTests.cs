using NUnit.Framework;
using YnabCli.Instructions.Extraction;
using YnabCli.Instructions.Indexers;

namespace YnabCli.Instructions.Tests;

[TestFixture]
public class InstructionTokenExtractorTests
{
    private InstructionTokenExtractor _legacyInstructionTokenExtractor;

    [SetUp]
    public void SetUp()
    {
        _legacyInstructionTokenExtractor = new InstructionTokenExtractor();
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionPrefixs()
    {
        var input = $"/command";

        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = 7,
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);
        
        Assert.That(result.PrefixToken, Is.EqualTo("/"));
    }
    
    [Test]
    public void GivenInputStringWithoutArguments_WhenParse_ReturnsInstructionWithoutArguments()
    {
        var input = $"/command";
        
        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = input.Length,
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);
        
        Assert.That(result.NameToken, Is.EqualTo("command"));
    }

    [Test]
    public void GivenInputStringWithSubCommand_WhenParse_ReturnsInstructionWithSubCommand()
    {
        var input = $"/command sub-command";
        
        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = 7,
            SubNameTokenIndexed = true,
            SubNameStartIndex = 9,
            SubNameEndIndex = input.Length
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);
        
        Assert.That(result.SubNameToken, Is.EqualTo("sub-command"));
    }

    [Test]
    public void GivenInputWithArguments_WhenParse_ReturnsInstructionsWithCorrectName()
    {
        var input = $"/command --argumentOne hello world";
        
        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = 8,
            ArgumentTokensIndexed = true,
            ArgumentTokensStartIndex = 9,
            ArgumentTokensEndIndex = input.Length,
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);
        
        Assert.That(result.NameToken, Is.EqualTo("command"));
    }
    
    
    [Test]
    public void GivenInputStringWithArgumentsAndNoValue_WhenParse_ReturnsInstructionWithEmptyArguments()
    {
        var argumentName = "argumentOne";
        
        var input = $"/command --{argumentName}";
        
        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = 7,
            ArgumentTokensIndexed = true,
            ArgumentTokensStartIndex = 9,
            ArgumentTokensEndIndex = input.Length,
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);
        
        var resultingArgument = result.ArgumentTokens.First();
        
        Assert.That(resultingArgument.Key, Is.EqualTo(argumentName));
        Assert.That(resultingArgument.Value, Is.Null);
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionWithOneStringArguments()
    {
        var argumentName = "argumentOne";
        var argumentValue = "hello world";
        
        var input = $"/command --{argumentName} {argumentValue}";
        
        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = 7,
            ArgumentTokensIndexed = true,
            ArgumentTokensStartIndex = 9,
            ArgumentTokensEndIndex = input.Length,
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);
        
        var resultingArgument = result.ArgumentTokens.First();
        
        Assert.That(resultingArgument.Key, Is.EqualTo(argumentName));
        Assert.That(resultingArgument.Value, Is.EqualTo(argumentValue));
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionWithTwoStringArguments()
    {
        var argumentOneName = "argumentOne";
        var argumentOneValue = "hello world";
        var argumentTwoName = "argumentTwo";
        var argumentTwoValue = "world hello";
        
        var input = $"/command --{argumentOneName} {argumentOneValue} --{argumentTwoName} {argumentTwoValue}";
        
        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = 7,
            ArgumentTokensIndexed = true,
            ArgumentTokensStartIndex = 9,
            ArgumentTokensEndIndex = input.Length,
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);

        var resultingArgumentOne = result.ArgumentTokens.FirstOrDefault();
        
        Assert.That(resultingArgumentOne.Key, Is.EqualTo(argumentOneName));
        Assert.That(resultingArgumentOne.Value, Is.EqualTo(argumentOneValue));

        var resultingArgumentTwo = result.ArgumentTokens.LastOrDefault();
        
        Assert.That(resultingArgumentTwo.Key, Is.EqualTo(argumentTwoName));
        Assert.That(resultingArgumentTwo.Value, Is.EqualTo(argumentTwoValue));
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionWithMultipleTypedArguments()
    {
        var argumentOneName = "argumentOne";
        var argumentOneValue = "hello world";
        var argumentTwoName = "argumentTwo";
        var argumentTwoValue = "1";
        
        var input = $"/command --{argumentOneName} {argumentOneValue} --{argumentTwoName} {argumentTwoValue}";
        
        var indexes = new InstructionTokenIndexes
        {
            PrefixTokenIndexed = true,
            PrefixTokenStartIndex = 0,
            PrefixTokenEndIndex = 1,
            NameTokenIndexed = true,
            NameTokenStartIndex = 1,
            NameTokenEndIndex = 7,
            ArgumentTokensIndexed = true,
            ArgumentTokensStartIndex = 9,
            ArgumentTokensEndIndex = input.Length,
        };
        
        var result = _legacyInstructionTokenExtractor.Extract(indexes, input);
        
        var resultingArgumentOne = result.ArgumentTokens.FirstOrDefault();
        
        Assert.That(resultingArgumentOne.Key, Is.EqualTo(argumentOneName));
        Assert.That(resultingArgumentOne.Value, Is.EqualTo(argumentOneValue));
        
        var resultingArgumentTwo = result.ArgumentTokens.LastOrDefault();
        
        Assert.That(resultingArgumentTwo.Key, Is.EqualTo(argumentTwoName));
        Assert.That(resultingArgumentTwo.Value, Is.EqualTo(argumentTwoValue));
    }
}
