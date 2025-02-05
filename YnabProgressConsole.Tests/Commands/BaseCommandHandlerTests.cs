using ConsoleTables;
using YnabProgress.ViewModels;
using YnabProgressConsole.Commands;

namespace YnabProgressConsole.Tests.Commands;

[TestFixture]
public class BaseCommandHandlerTests
{
    private class TestCommandHandler : BaseCommandHandler
    {
        public ConsoleTable TestCompile(ViewModel vm) => Compile(vm);
    }

    [Test]
    public void GivenViewModel_WhenCompile_ShouldReturnConsoleTable()
    {
        var vm = new ViewModel
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