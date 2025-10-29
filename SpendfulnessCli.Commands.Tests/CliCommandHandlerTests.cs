using Cli.Abstractions;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using ConsoleTables;

namespace Cli.Spendfulness.Commands.Tests;

[TestFixture]
public class CliCommandHandlerTests
{
    private class TestCliCommandHandler : CliCommandHandler
    {
        public CliCommandOutcome TestCompile(CliTable vm) => Compile(vm);
    }

    [Test]
    public void GivenViewModel_WhenCompile_ShouldReturnConsoleTable()
    {
        var viewModel = new CliTable
        {
            Columns = ["Column1", "Column2", "Column3"],
            Rows = [new List<object>() { "Row1", "Row2", "Row3" }]
        };
        
        var result = new TestCliCommandHandler().TestCompile(viewModel);

        var expectedConsoleTable = new ConsoleTable();
        expectedConsoleTable.AddColumn(viewModel.Columns.ToArray());
        expectedConsoleTable.AddRow("Row1", "Row2", "Row3");
        
        Assert.That(result.ToString(), Is.EqualTo(expectedConsoleTable.ToString()));
    }
}