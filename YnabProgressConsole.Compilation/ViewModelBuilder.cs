namespace YnabProgressConsole.Compilation;

public abstract class ViewModelBuilder
{
    protected List<string> _columnNames = [];
    protected string _sortColumnName = string.Empty;
    protected SortOrder _sortOrder = SortOrder.Ascending;
    
    public IViewModelBuilder AddColumnNames(params string[] columnNames)
    {
        _columnNames = columnNames.ToList();
        return this as IViewModelBuilder;
    }

    public IViewModelBuilder AddSortColumnName(string columnName)
    {
        _sortColumnName = columnName;
        return this as IViewModelBuilder;
    }

    public IViewModelBuilder AddSortOrder(SortOrder sortOrder)
    {
        _sortOrder = sortOrder;
        return this as IViewModelBuilder;
    }
}