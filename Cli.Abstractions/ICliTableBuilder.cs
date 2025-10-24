namespace Cli.Abstractions;

public interface ICliTableBuilder<TAggregation> where TAggregation : notnull
{
    ICliTableBuilder<TAggregation> WithAggregator(CliAggregator<TAggregation> aggregator);
    
    ICliTableBuilder<TAggregation> WithSortOrder(CliTableSortOrder viewModelSortOrder);

    ICliTableBuilder<TAggregation> WithRowCount(bool showRowCount);
    
    CliTable Build();
}