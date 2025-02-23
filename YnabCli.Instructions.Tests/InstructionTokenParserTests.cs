using NUnit.Framework;

namespace YnabCli.Instructions.Tests;

[TestFixture]
public class InstructionTokenParserTests
{
    private InstructionTokenParser _instructionTokenParser;

    [SetUp]
    public void SetUp()
    {
        _instructionTokenParser = new InstructionTokenParser();
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionPrefixs()
    {
        var input = $"/command";
        
        var result = _instructionTokenParser.Parse(input);
        
        Assert.That(result.CommandPrefixToken, Is.EqualTo("/"));
    }
    
    [Test]
    public void GivenInputStringWithoutArguments_WhenaParse_ReturnsInstructionWithoutArguments()
    {
        var input = $"/command";
        
        var result = _instructionTokenParser.Parse(input);
        
        Assert.That(result.CommandNameToken, Is.EqualTo("command"));
    }

    [Test]
    public void GivenInputWithArguments_WhenParse_ReturnsInstructionsWithCorrectName()
    {
        var input = $"/command --argumentOne hello world";
        
        var result = _instructionTokenParser.Parse(input);
        
        Assert.That(result.CommandNameToken, Is.EqualTo("command"));
    }
    
    
    [Test]
    public void GivenInputStringWithArgumentsAndNoValue_WhenParse_ReturnsInstructionWithEmptyArguments()
    {
        var argumentName = "argumentOne";
        
        var input = $"/command --{argumentName}";
        
        var result = _instructionTokenParser.Parse(input);
        
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
        
        var result = _instructionTokenParser.Parse(input);
        
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
        
        var result = _instructionTokenParser.Parse(input);

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
        
        var result = _instructionTokenParser.Parse(input);
        
        var resultingArgumentOne = result.ArgumentTokens.FirstOrDefault();
        
        Assert.That(resultingArgumentOne.Key, Is.EqualTo(argumentOneName));
        Assert.That(resultingArgumentOne.Value, Is.EqualTo(argumentOneValue));
        
        var resultingArgumentTwo = result.ArgumentTokens.LastOrDefault();
        
        Assert.That(resultingArgumentTwo.Key, Is.EqualTo(argumentTwoName));
        Assert.That(resultingArgumentTwo.Value, Is.EqualTo(argumentTwoValue));
    }
}
