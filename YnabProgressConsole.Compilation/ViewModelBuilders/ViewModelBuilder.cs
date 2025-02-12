using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.ViewModelBuilders;

public abstract class ViewModelBuilder
{
    protected List<string> ColumnNames = [];
    protected ViewModelSortOrder ViewModelSortOrder = ViewModelSortOrder.Ascending;
    protected bool ShowRowCount = true;
    
    public IViewModelBuilder AddColumnNames(List<string> columnNames)
    {
        ColumnNames = columnNames;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder AddSortOrder(ViewModelSortOrder viewModelSortOrder)
    {
        ViewModelSortOrder = viewModelSortOrder;
        return GetCurrentBuilder();
    }

    public IViewModelBuilder AddRowCount(bool showRowCount)
    {
        ShowRowCount = showRowCount;
        return GetCurrentBuilder();
    }

    private IViewModelBuilder GetCurrentBuilder()
    {
        var current = this as IViewModelBuilder;
        if (current is null)
        {
            throw new Exception("Attempted to return a non-IViewModelBuilder superclass of ViewModelBuilder");
        }
        
        return current;
    }
}