using SpendfulnessCli.Commands.Handlers;
using ConsoleTables;
using SpendfulnessCli.Commands;

namespace Cli.Spendfulness.Commands.Tests.CommandList;

[TestFixture]
public class CommandListCliCommandHandlerTests
{
    [Test]
    public async Task GivenCommandListCommand_WhenHandle_ShouldReturnConsoleTable()
    {
        var handler = new CommandListCliCommandHandler();
        var command = new CommandListCliCommand();
        
        var result = await handler.Handle(command, CancellationToken.None);

        var expectedConsoleTable = new ConsoleTable();
        expectedConsoleTable.AddColumn(["Command", "Description"]);
        expectedConsoleTable.AddRow("/command-list", "Gets a list of commands");
        
        Assert.That(result.ToString(), Is.EqualTo(expectedConsoleTable.ToString()));
    }
}