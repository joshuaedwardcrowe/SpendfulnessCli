using YnabCli.Aggregation.Aggregator;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.ViewModelBuilders;

public abstract class ViewModelBuilder<TAggregator, TAggregation> : IViewModelBuilder<TAggregator, TAggregation>
    where TAggregator : Aggregator<TAggregation>
    where TAggregation : notnull
{
    protected ViewModelSortOrder ViewModelSortOrder = ViewModelSortOrder.Ascending;
    private TAggregator? _aggregator;
    private bool _showRowCount = true;

    public IViewModelBuilder<TAggregator, TAggregation> WithAggregator(TAggregator aggregator)
    {
        _aggregator = aggregator;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder<TAggregator, TAggregation> WithSortOrder(ViewModelSortOrder viewModelSortOrder)
    {
        ViewModelSortOrder = viewModelSortOrder;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder<TAggregator, TAggregation> WithRowCount(bool showRowCount)
    {
        _showRowCount = showRowCount;
        return GetCurrentBuilder();
    }

    public ViewModel Build()
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

    private ViewModel BuildViewModel(List<string> columnNames, List<List<object>> rows)
    {
        return new ViewModel
        {
            ShowRowCount = _showRowCount,
            Columns = columnNames,
            Rows = rows,
        };
    }

    private IViewModelBuilder<TAggregator, TAggregation> GetCurrentBuilder()
    {
        var current = this as IViewModelBuilder<TAggregator, TAggregation>;
        if (current is null)
        {
            throw new Exception("Attempted to return a non-IViewModelBuilder superclass of ViewModelBuilder");
        }
        
        return current;
    }
}