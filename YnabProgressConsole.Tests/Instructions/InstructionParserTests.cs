using YnabProgressConsole.Instructions;
using YnabProgressConsole.Instructions.InstructionArgumentBuilders;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Tests.Instructions;

[TestFixture]
public class InstructionParserTests
{
    private IEnumerable<IInstructionArgumentBuilder> _argumentBuilders;
    private InstructionParser _parser;

    [SetUp]
    public void SetUp()
    {
        _argumentBuilders = new List<IInstructionArgumentBuilder>
        {
            new StringInstructionArgumentBuilder(),
            new IntInstructionArgumentBuilder(),
        };
        
        _parser = new InstructionParser(_argumentBuilders);
    }
    
    [Test]
    public void GivenInputStringWithoutArguments_WhenaParse_ReturnsInstructionWithoutArguments()
    {
        var input = $"/command";
        
        var result = _parser.Parse(input);
        
        Assert.That(result.InstructionName, Is.EqualTo("command"));
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionWithOneStringArguments()
    {
        var argumentName = "argumentOne";
        var argumentValue = "hello world";
        
        var input = $"/command --{argumentName} {argumentValue}";
        
        var result = _parser.Parse(input);
        
        var resultingArgument = result.Arguments
            .OfType<TypedInstructionArgument<string>>()
            .Single();
        
        Assert.That(resultingArgument.ArgumentName, Is.EqualTo(argumentName));
        Assert.That(resultingArgument.ArgumentValue, Is.EqualTo(argumentValue));
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionWithTwoStringArguments()
    {
        var argumentOneName = "argumentOne";
        var argumentOneValue = "hello world";
        var argumentTwoName = "argumentTwo";
        var argumentTwoValue = "world hello";
        
        var input = $"/command --{argumentOneName} {argumentOneValue} --{argumentTwoName} {argumentTwoValue}";
        
        var result = _parser.Parse(input);
        
        var resultingArgumentOne = result.Arguments
            .OfType<TypedInstructionArgument<string>>()
            .FirstOrDefault();
        
        Assert.That(resultingArgumentOne.ArgumentName, Is.EqualTo(argumentOneName));
        Assert.That(resultingArgumentOne.ArgumentValue, Is.EqualTo(argumentOneValue));
        
        var resultingArgumentTwo = result.Arguments
            .OfType<TypedInstructionArgument<string>>()
            .Last();
        
        Assert.That(resultingArgumentTwo.ArgumentName, Is.EqualTo(argumentTwoName));
        Assert.That(resultingArgumentTwo.ArgumentValue, Is.EqualTo(argumentTwoValue));
    }
    
    [Test]
    public void GivenInputString_WhenParse_ReturnsInstructionWithMultipleTypedArguments()
    {
        var argumentOneName = "argumentOne";
        var argumentOneValue = "hello world";
        var argumentTwoName = "argumentTwo";
        var argumentTwoValue = 1;
        
        var input = $"/command --{argumentOneName} {argumentOneValue} --{argumentTwoName} {argumentTwoValue}";
        
        var result = _parser.Parse(input);
        
        var resultingArgumentOne = result.Arguments
            .OfType<TypedInstructionArgument<string>>()
            .FirstOrDefault();
        
        Assert.That(resultingArgumentOne.ArgumentName, Is.EqualTo(argumentOneName));
        Assert.That(resultingArgumentOne.ArgumentValue, Is.EqualTo(argumentOneValue));
        
        var resultingArgumentTwo = result.Arguments
            .OfType<TypedInstructionArgument<int>>()
            .Last();
        
        Assert.That(resultingArgumentTwo.ArgumentName, Is.EqualTo(argumentTwoName));
        Assert.That(resultingArgumentTwo.ArgumentValue, Is.EqualTo(argumentTwoValue));
    }
}