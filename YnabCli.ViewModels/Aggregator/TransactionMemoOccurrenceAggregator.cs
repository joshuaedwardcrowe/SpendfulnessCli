using Ynab;
using Ynab.Extensions;
using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Extensions;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.Aggregator;

public class TransactionMemoOccurrenceAggregator(IEnumerable<Transaction> transactions)
    : ListAggregator<TransactionMemoOccurrenceAggregate>(transactions)
{
    protected override IEnumerable<TransactionMemoOccurrenceAggregate> ListAggregate() =>
        Transactions
            .FilterToSpending()
            .GroupByPayeeName()
            .GroupByMemoOccurence()
            .AggregateMemoOccurrences();
}

public abstract class ListAggregator<TAggregate> : Aggregator<IEnumerable<TAggregate>>
{
    private readonly List<Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>>> _operationFunctions = [];

    protected ListAggregator(IEnumerable<Transaction> transactions)
        : base(transactions)
    {
    }

    public override IEnumerable<TAggregate> Aggregate()
    {
        // TODO: Pre-aggregation filters
        
        var specificAggregation = ListAggregate();

        // TODO: Post-aggregation filters
        foreach (var operationFunction in _operationFunctions)
        {
            specificAggregation = operationFunction(specificAggregation);
        }
        
        return specificAggregation;
    }

    public ListAggregator<TAggregate> AddAggregationOperation(Func<IEnumerable<TAggregate>, IEnumerable<TAggregate>> filter)
    {
        _operationFunctions.Add(filter);
        return this;
    }

    protected abstract IEnumerable<TAggregate> ListAggregate();
}