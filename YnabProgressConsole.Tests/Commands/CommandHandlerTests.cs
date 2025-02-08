using ConsoleTables;
using YnabProgressConsole.Commands;
using YnabProgressConsole.Compilation;

namespace YnabProgressConsole.Tests.Commands;

[TestFixture]
public class CommandHandlerTests
{
    private class TestCommandHandler : CommandHandler
    {
        public ConsoleTable TestCompile(ConsoleTableViewModel vm) => Compile(vm);
    }

    [Test]
    public void GivenViewModel_WhenCompile_ShouldReturnConsoleTable()
    {
        var vm = new ConsoleTableViewModel()
        {
            Columns = ["Column1", "Column2", "Column3"],
            Rows = [new List<object>() { "Row1", "Row2", "Row3" }]
        };
        
        var result = new TestCommandHandler().TestCompile(vm);

        var expectedConsoleTable = new ConsoleTable();
        expectedConsoleTable.AddColumn(vm.Columns.ToArray());
        expectedConsoleTable.AddRow("Row1", "Row2", "Row3");
        
        Assert.That(result.ToString(), Is.EqualTo(expectedConsoleTable.ToString()));
    }
}