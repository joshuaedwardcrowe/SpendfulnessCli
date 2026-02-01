using KitCli.Abstractions.Aggregators;
using YnabSharp;

namespace SpendfulnessCli.Commands.Reusable.Filter.Transactions.PayeeName.Equals;

public record FilterTransactionsOnPayeeNameEqualsCliCommand : FilterTransactionsCliCommand
{
    public new static class ArgumentNames
    {
        public const string Is = "is";
    }
    
    public FilterTransactionsOnPayeeNameEqualsCliCommand(
        CliListAggregator<Transaction> aggregator,
        string filterOn,
        string payeeName) 
        : base(aggregator, filterOn)
    {
        PayeeName = payeeName;
    }
    
    public string PayeeName { get; }
}