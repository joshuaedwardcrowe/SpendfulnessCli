namespace YnabProgressConsole.Compilation;

public interface IViewModelBuilder<TGroup>
{
    public IViewModelBuilder<TGroup> AddGroups(IEnumerable<TGroup> groups);
    
    public IViewModelBuilder<TGroup> AddColumnNames(params string[] columnNames);
    
    public IViewModelBuilder<TGroup> AddSortColumnName(string columnName);
    
    public IViewModelBuilder<TGroup> AddSortOrder(SortOrder sortOrder);

    public ViewModel Build();
}