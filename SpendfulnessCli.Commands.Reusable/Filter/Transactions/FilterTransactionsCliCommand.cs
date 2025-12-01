using Cli.Abstractions.Aggregators;
using Ynab;

namespace SpendfulnessCli.Commands.Reusable.Filter.Transactions;

public record FilterTransactionsCliCommand : FilterCliCommand
{
    public static class FilterNames
    {
        public const string PayeeName = "Payee";
        
        public static string[] All => [PayeeName];
    }
    
    public FilterTransactionsCliCommand(CliListAggregator<Transaction> aggregator, string filterOn) : base(filterOn)
    {
        Aggregator = aggregator;
    }
    
    public CliListAggregator<Transaction> Aggregator { get; }
}