using Ynab;

namespace YnabCli.Aggregation.Aggregator.ListAggregators;

public class TransactionAggregator(IEnumerable<Transaction> transactions) : ListAggregator<Transaction>(transactions)
{
    protected override IEnumerable<Transaction> ListAggregate() => Transactions;
}