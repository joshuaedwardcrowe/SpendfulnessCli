using Cli.Abstractions;
using Cli.Abstractions.Aggregators;
using Cli.Abstractions.Tables;

namespace SpendfulnessCli.CliTables.ViewModelBuilders;

public abstract class CliTableBuilder<TAggregate> : ICliTableBuilder<TAggregate>
    where TAggregate : notnull
{
    protected CliTableSortOrder CliTableSortOrder = CliTableSortOrder.Ascending;
    private CliListAggregator<TAggregate>? _aggregator;
    private bool _showRowCount = true;

    public ICliTableBuilder<TAggregate> WithAggregator(CliListAggregator<TAggregate> listAggregator)
    {
        _aggregator = listAggregator;
        return GetCurrentBuilder();
    }

    public ICliTableBuilder<TAggregate> WithSortOrder(CliTableSortOrder cliTableSortOrder)
    {
        CliTableSortOrder = cliTableSortOrder;
        return GetCurrentBuilder();
    }

    public ICliTableBuilder<TAggregate> WithRowCount(bool showRowCount)
    {
        _showRowCount = showRowCount;
        return GetCurrentBuilder();
    }

    public CliTable Build()
    {
        if (_aggregator is null)
        {
            // This is genuinely an exceptional circumstance.
            throw new InvalidOperationException("You must provide at least one aggregator");
        }

        var evaluation = _aggregator
            .Aggregate()
            .ToList();

        var columns = BuildColumnNames(evaluation);
        var rows = BuildRows(evaluation);

        return BuildViewModel(columns, rows);
    }
    
    protected virtual List<string> BuildColumnNames(IEnumerable<TAggregate> aggregates) => [];
    
    protected abstract List<List<object>> BuildRows(IEnumerable<TAggregate> aggregates);

    private CliTable BuildViewModel(List<string> columnNames, List<List<object>> rows)
    {
        return new CliTable
        {
            ShowRowCount = _showRowCount,
            Columns = columnNames,
            Rows = rows,
        };
    }

    private ICliTableBuilder<TAggregate> GetCurrentBuilder()
    {
        if (this is not ICliTableBuilder<TAggregate> current)
        {
            throw new Exception("Attempted to return a non-IViewModelBuilder superclass of ViewModelBuilder");
        }
        
        return current;
    }
}