using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public abstract class ViewModelBuilder<TEvaluator, TEvaluation> : IViewModelBuilder<TEvaluator, TEvaluation>
    where TEvaluator : YnabEvaluator<TEvaluation>
    where TEvaluation : notnull
{
    protected ViewModelSortOrder ViewModelSortOrder = ViewModelSortOrder.Ascending;
    private TEvaluator? _evaluator;
    private List<string> _columnNames = [];
    private bool _showRowCount = true;

    public IViewModelBuilder<TEvaluator, TEvaluation> AddEvaluator(TEvaluator evaluator)
    {
        _evaluator = evaluator;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder<TEvaluator, TEvaluation> AddColumnNames(List<string> columnNames)
    {
        _columnNames = columnNames;
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
        
        var rows = BuildRows(_evaluator);

        return BuildViewModel(rows);
    }
    
    protected abstract List<List<object>> BuildRows(TEvaluator evaluator);

    private ViewModel BuildViewModel(List<List<object>> rows)
    {
        return new ViewModel
        {
            ShowRowCount = _showRowCount,
            Columns = _columnNames,
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