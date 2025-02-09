using NUnit.Framework;
using YnabProgressConsole.Instructions.InstructionArgumentBuilders;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions.Tests.InstructionArgumentBuilders;

[TestFixture]
public class IntInstructionArgumentBuilderTests
{
    private IntInstructionArgumentBuilder _intInstructionArgumentBuilder;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _intInstructionArgumentBuilder = new IntInstructionArgumentBuilder();
    }
    
    
    [Test]
    public void GivenIntArgumentValue_WhenFor_ShouldReturnTrue()
    {
        var result = _intInstructionArgumentBuilder.For("1");
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GivenWrongArgumentValue_WhenFor_ShouldReturnFalse()
    {
        var result = _intInstructionArgumentBuilder.For("hello test");
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void GivenIntArgumentValue_WhenCreate_ShouldReturnInstructionArgument()
    {
        var result = _intInstructionArgumentBuilder.Create(string.Empty, "1");

        var typed = result as TypedInstructionArgument<int>;
        
        Assert.That(typed, Is.Not.Null);
        Assert.That(typed.ArgumentValue, Is.EqualTo(1));
    }

    [Test]
    public void GivenNonIntArgumentValue_WhenCreate_WillThrowException()
    {
        void CreateArgument() => _intInstructionArgumentBuilder.Create(string.Empty, "hello world");

        Assert.Throws<ArgumentException>(CreateArgument);
    }
}