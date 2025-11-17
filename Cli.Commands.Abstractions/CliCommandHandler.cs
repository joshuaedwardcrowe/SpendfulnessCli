using Cli.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using ConsoleTables;

namespace Cli.Commands.Abstractions;

public abstract class CliCommandHandler
{
    protected static CliCommandTableOutcome Compile(CliTable cliTable)
    {
        var table = new ConsoleTable
        {
            Options =
            {
                EnableCount = cliTable.ShowRowCount
            }
        };

        table.AddColumn(cliTable.Columns.ToArray());
       
        foreach (var row in cliTable.Rows)
            table.AddRow(row.ToArray());
        
        return new CliCommandTableOutcome(table);
    }
    
    protected static CliCommandAggregatorOutcome<TAggregate> Compile<TAggregate>(CliAggregator<TAggregate> aggregator)
        => new(aggregator);

    protected static CliCommandOutputOutcome Compile(string message) => new(message);
}