using ConsoleTables;
using YnabProgressConsole.Commands.CommandList;

namespace YnabProgressConsole.Commands.Tests.Commands.CommandList;

[TestFixture]
public class CommandListCommandHandlerTests
{
    [Test]
    public async Task GivenCommandListCommand_WhenHandle_ShouldReturnConsoleTable()
    {
        var handler = new CommandListCommandHandler();
        var command = new CommandListCommand();
        
        var result = await handler.Handle(command, CancellationToken.None);

        var expectedConsoleTable = new ConsoleTable();
        expectedConsoleTable.AddColumn(["Command", "Description"]);
        expectedConsoleTable.AddRow("/command-list", "Gets a list of commands");
        
        Assert.That(result.ToString(), Is.EqualTo(expectedConsoleTable.ToString()));
    }
}