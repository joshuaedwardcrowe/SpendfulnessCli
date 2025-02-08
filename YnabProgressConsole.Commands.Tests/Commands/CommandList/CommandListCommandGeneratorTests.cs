using YnabProgressConsole.Commands.CommandList;
using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.Tests.Commands.CommandList;

[TestFixture]
public class CommandListCommandGeneratorTests
{
    [Test]
    public void GivenAnyArguments_WhenGenerate_ReturnsCommandListCommand()
    {
        var generator = new CommandListCommandGenerator();

        var arguments = new List<InstructionArgument>();
        
        var result = generator.Generate(arguments);
        
        Assert.That(result, Is.TypeOf<CommandListCommand>());
    }
}