using Cli.Instructions.Arguments;
using Cli.Instructions.Builders;
using NUnit.Framework;

namespace Cli.Instructions.Tests.InstructionArgumentBuilders;

[TestFixture]
public class StringCliInstructionArgumentBuilderTests
{
    [Test]
    public void GivenStringArgumentValue_WhenCreate_ShouldReturnInstructionArgument()
    {
        var builder = new StringCliInstructionArgumentBuilder();
        
        var result = builder.Create(string.Empty, "test test test");

        var typed = result as TypedConsoleInstructionArgument<string>;
        
        Assert.That(typed, Is.Not.Null);
        Assert.That(typed.ArgumentValue, Is.EqualTo("test test test"));
    }
    
    [Test]
    public void GivenStringArgumentValue_WhenFor_ShouldReturnTrue()
    {
        var builder = new StringCliInstructionArgumentBuilder();
        
        var result = builder.For("hello hello hello");
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GivenWrongArgumentValue_WhenFor_ShouldReturnFalse()
    {
        var builder = new StringCliInstructionArgumentBuilder();
        
        var result = builder.For("1");
        
        Assert.That(result, Is.False);
    }
}