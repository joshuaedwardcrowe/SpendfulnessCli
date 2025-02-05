using YnabProgressConsole.Commands.CommandList;

namespace YnabProgressConsole.Tests.Commands.CommandList;

[TestFixture]
public class CommandListCommandGeneratorTests
{
    [Test]
    public void GivenAnyArguments_WhenGenerate_ReturnsCommandListCommand()
    {
        var generator = new CommandListCommandGenerator();

        var result = generator.Generate(["crash", "bang", "whollop"]);
        
        Assert.That(result, Is.TypeOf<CommandListCommand>());
    }
}