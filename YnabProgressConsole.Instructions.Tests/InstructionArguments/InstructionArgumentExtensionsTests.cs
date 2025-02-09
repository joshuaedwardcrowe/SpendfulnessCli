using NUnit.Framework;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Instructions.Tests.InstructionArguments;

[TestFixture]
public class InstructionArgumentExtensionsTests
{
    private List<InstructionArgument> _arguments;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _arguments =
        [
            new TypedInstructionArgument<string>("argumentOne", "hello"),
            new TypedInstructionArgument<int>("argumentTwo", 12345),
            new TypedInstructionArgument<bool>("argumentThree", true),
            new TypedInstructionArgument<decimal>("argumentFour", 12.0m)
        ];
    }
    
    [TestCase]
    public void GivenArgumentWithSameName_WhenOfType_ReturnsArgumentOfType()
    {
        var result = _arguments.OfType<string>("argumentOne");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.ArgumentValue, Is.EqualTo("hello"));
    }

    [Test]
    public void GivenNoArgumentWithSameName_WhenOfType_ReturnsNull()
    {
        var result = _arguments.OfType<int>("argumentFive");
        
        Assert.That(result, Is.Null);
    }
}