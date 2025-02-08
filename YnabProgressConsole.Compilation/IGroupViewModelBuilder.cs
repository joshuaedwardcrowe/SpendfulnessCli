using Ynab.Aggregates;

namespace YnabProgressConsole.Compilation;

public interface IGroupViewModelBuilder<in TGroup> : IViewModelBuilder
{
    public IGroupViewModelBuilder<TGroup> AddGroups(IEnumerable<TGroup> groups);
}

public interface IAggregateViewModelBuilder<in TAggregation, TAggregate>  : IViewModelBuilder 
    where TAggregation : YnabAggregation<TAggregate>
    where TAggregate : class
{
    IAggregateViewModelBuilder<TAggregation, TAggregate> AddAggregation(TAggregation aggregate);
}

public interface IViewModelBuilder
{
    public IViewModelBuilder AddColumnNames(params string[] columnNames);
    
    public IViewModelBuilder AddSortColumnName(string columnName);
    
    public IViewModelBuilder AddSortOrder(SortOrder sortOrder);

    public ViewModel Build();
} 