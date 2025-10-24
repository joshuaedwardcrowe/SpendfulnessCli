using Cli.Instructions.Arguments;
using Cli.Instructions.Builders;
using NUnit.Framework;

namespace Cli.Instructions.Tests.InstructionArgumentBuilders;

[TestFixture]
public class IntCliInstructionArgumentBuilderTests
{
    private IntCliInstructionArgumentBuilder _intCliInstructionArgumentBuilder;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _intCliInstructionArgumentBuilder = new IntCliInstructionArgumentBuilder();
    }
    
    
    [Test]
    public void GivenIntArgumentValue_WhenFor_ShouldReturnTrue()
    {
        var result = _intCliInstructionArgumentBuilder.For("1");
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GivenWrongArgumentValue_WhenFor_ShouldReturnFalse()
    {
        var result = _intCliInstructionArgumentBuilder.For("hello test");
        
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void GivenIntArgumentValue_WhenCreate_ShouldReturnInstructionArgument()
    {
        var result = _intCliInstructionArgumentBuilder.Create(string.Empty, "1");

        var typed = result as TypedConsoleInstructionArgument<int>;
        
        Assert.That(typed, Is.Not.Null);
        Assert.That(typed.ArgumentValue, Is.EqualTo(1));
    }
}