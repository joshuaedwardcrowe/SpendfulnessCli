using SpendfulnessCli.Aggregation.Aggregator;
using Ynab;

namespace Spendfulness.Csv;

public interface ICsvBuilder<TAggregate> 
{
    ICsvBuilder<TAggregate> WithBudget(Budget budget);
    
    ICsvBuilder<TAggregate> WithAggregator(YnabListAggregator<TAggregate> aggregator);
    
    Csv Build();
}