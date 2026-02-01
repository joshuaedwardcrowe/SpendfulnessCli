using YnabSharp;

namespace Spendfulness.Aggregation.Aggregator.ListAggregators;

public class TransactionPagedListAggregator : YnabListAggregator<Transaction>
{
    public TransactionPagedListAggregator(IEnumerable<Transaction> transactions, int pageNumber, int pageSize)
        : base(transactions, pageSize, pageNumber)
    {
    }

    protected override IEnumerable<Transaction> GenerateAggregate() => Transactions;
}