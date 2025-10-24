using Ynab;

namespace YnabCli.Aggregation.Aggregator.ListAggregators;

public class TransactionYnabAggregator(IEnumerable<Transaction> transactions) : ListYnabAggregator<Transaction>(transactions)
{
    protected override IEnumerable<Transaction> ListAggregate() => Transactions;
}