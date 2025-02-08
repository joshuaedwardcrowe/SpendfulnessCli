using YnabProgressConsole.Instructions.InstructionArgumentBuilders;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Tests.Instructions.InstructionArgumentBuilders;

[TestFixture]
public class IntInstructionArgumentBuilderTests
{
    [Test]
    public void GivenIntArgumentValue_WhenCreate_ShouldReturnInstructionArgument()
    {
        var builder = new IntInstructionArgumentBuilder();
        
        var result = builder.Create(string.Empty, "1");

        var typed = result as TypedInstructionArgument<int>;
        Assert.That(typed.ArgumentValue, Is.EqualTo(1));
    }
    
    [Test]
    public void GivenIntArgumentValue_WhenFor_ShouldReturnTrue()
    {
        var builder = new IntInstructionArgumentBuilder();
        
        var result = builder.For("1");
        
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void GivenWrongArgumentValue_WhenFor_ShouldReturnFalse()
    {
        var builder = new IntInstructionArgumentBuilder();
        
        var result = builder.For("hello test");
        
        Assert.That(result, Is.False);
    }
}