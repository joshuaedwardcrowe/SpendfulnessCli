using Spendfulness.Aggregation.Aggregator;
using YnabSharp;

namespace Spendfulness.Csv;

public interface ICsvBuilder<TAggregate> 
{
    ICsvBuilder<TAggregate> WithBudget(Budget budget);
    
    ICsvBuilder<TAggregate> WithAggregator(YnabListAggregator<TAggregate> aggregator);
    
    Csv Build();
}