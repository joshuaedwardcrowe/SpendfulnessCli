using Cli.Abstractions;

namespace Spendfulness.Cli.CliTables.ViewModelBuilders;

public abstract class CliTableBuilder<TAggregation> : ICliTableBuilder<TAggregation>
    where TAggregation : notnull
{
    protected CliTableSortOrder CliTableSortOrder = CliTableSortOrder.Ascending;
    private CliAggregator<TAggregation>? _aggregator;
    private bool _showRowCount = true;

    public ICliTableBuilder<TAggregation> WithAggregator(CliAggregator<TAggregation> ynabAggregator)
    {
        _aggregator = ynabAggregator;
        return GetCurrentBuilder();
    }

    public ICliTableBuilder<TAggregation> WithSortOrder(CliTableSortOrder cliTableSortOrder)
    {
        CliTableSortOrder = cliTableSortOrder;
        return GetCurrentBuilder();
    }

    public ICliTableBuilder<TAggregation> WithRowCount(bool showRowCount)
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

        var evaluation = _aggregator.Aggregate();

        var columns = BuildColumnNames(evaluation);
        var rows = BuildRows(evaluation);

        return BuildViewModel(columns, rows);
    }
    
    protected virtual List<string> BuildColumnNames(TAggregation evaluation) => [];
    
    protected abstract List<List<object>> BuildRows(TAggregation aggregates);

    private CliTable BuildViewModel(List<string> columnNames, List<List<object>> rows)
    {
        return new CliTable
        {
            ShowRowCount = _showRowCount,
            Columns = columnNames,
            Rows = rows,
        };
    }

    private ICliTableBuilder<TAggregation> GetCurrentBuilder()
    {
        if (this is not ICliTableBuilder<TAggregation> current)
        {
            throw new Exception("Attempted to return a non-IViewModelBuilder superclass of ViewModelBuilder");
        }
        
        return current;
    }
}