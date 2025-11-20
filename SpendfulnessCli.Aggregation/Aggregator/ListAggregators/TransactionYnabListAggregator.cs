using Ynab;

namespace SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

public class TransactionYnabListAggregator(IEnumerable<Transaction> transactions) : YnabListAggregator<Transaction>(transactions)
{
    protected override IEnumerable<Transaction> GenerateAggregate() => Transactions;
}