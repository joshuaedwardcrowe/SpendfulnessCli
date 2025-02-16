using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public abstract class ViewModelBuilder<TEvaluator, TEvaluation> : IViewModelBuilder<TEvaluator, TEvaluation>
    where TEvaluator : Aggregator<TEvaluation>
    where TEvaluation : notnull
{
    protected List<string> ColumnNames = [];
    protected ViewModelSortOrder ViewModelSortOrder = ViewModelSortOrder.Ascending;
    private TEvaluator? _evaluator;
    private bool _showRowCount = true;

    public IViewModelBuilder<TEvaluator, TEvaluation> AddAggregator(TEvaluator evaluator)
    {
        _evaluator = evaluator;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder<TEvaluator, TEvaluation> AddColumnNames(List<string> columnNames)
    {
        ColumnNames = columnNames;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder<TEvaluator, TEvaluation> AddSortOrder(ViewModelSortOrder viewModelSortOrder)
    {
        ViewModelSortOrder = viewModelSortOrder;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder<TEvaluator, TEvaluation> AddRowCount(bool showRowCount)
    {
        _showRowCount = showRowCount;
        return GetCurrentBuilder();
    }

    public ViewModel Build()
    {
        if (_evaluator is null)
        {
            throw new InvalidOperationException("You must provide at least one evaluator");
        }
        
        var evaluation = _evaluator.Evaluate();

        var columns = BuildColumnNames(evaluation);
        var rows = BuildRows(evaluation);

        return BuildViewModel(columns, rows);
    }
    
    protected virtual List<string> BuildColumnNames(TEvaluation evaluation) => ColumnNames;
    
    protected abstract List<List<object>> BuildRows(TEvaluation evaluation);

    private ViewModel BuildViewModel(List<string> columnNames, List<List<object>> rows)
    {
        return new ViewModel
        {
            ShowRowCount = _showRowCount,
            Columns = columnNames,
            Rows = rows,
        };
    }

    private IViewModelBuilder<TEvaluator, TEvaluation> GetCurrentBuilder()
    {
        var current = this as IViewModelBuilder<TEvaluator, TEvaluation>;
        if (current is null)
        {
            throw new Exception("Attempted to return a non-IViewModelBuilder superclass of ViewModelBuilder");
        }
        
        return current;
    }
}