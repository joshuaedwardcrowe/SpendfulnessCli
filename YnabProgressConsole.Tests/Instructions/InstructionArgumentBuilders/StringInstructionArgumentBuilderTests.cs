using YnabProgressConsole.Instructions.InstructionArgumentBuilders;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Tests.Instructions.InstructionArgumentBuilders;

[TestFixture]
public class StringInstructionArgumentBuilderTests
{
    [Test]
    public void GivenStringArgumentValue_WhenCreate_ShouldReturnInstructionArgument()
    {
        var builder = new StringInstructionArgumentBuilder();
        
        var result = builder.Create(string.Empty, "test test test");

        var typed = result as TypedInstructionArgument<string>;
        Assert.That(typed.ArgumentValue, Is.EqualTo("test test test"));
    }
    
    [Test]
    public void GivenStringArgumentValue_WhenFor_ShouldReturnTrue()
    {
        var builder = new StringInstructionArgumentBuilder();
        
        var result = builder.For("hello hello hello");
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GivenWrongArgumentValue_WhenFor_ShouldReturnFalse()
    {
        var builder = new StringInstructionArgumentBuilder();
        
        var result = builder.For("1");
        
        Assert.That(result, Is.False);
    }
}