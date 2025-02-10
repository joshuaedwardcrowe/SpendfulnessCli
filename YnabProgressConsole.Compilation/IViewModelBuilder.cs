namespace YnabProgressConsole.Compilation;

public interface IViewModelBuilder
{
    public IViewModelBuilder AddColumnNames(List<string> columnNames);
    
    public IViewModelBuilder AddSortColumnName(string columnName);
    
    public IViewModelBuilder AddSortOrder(ViewModelSortOrder viewModelSortOrder);

    public ViewModel Build();
}