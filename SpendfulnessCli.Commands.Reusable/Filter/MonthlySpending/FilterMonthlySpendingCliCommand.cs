using Cli.Abstractions.Aggregators;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public record FilterMonthlySpendingCliCommand : FilterCliCommand
{
    public static class FilterNames
    {
        public const string TotalAmount = "Total Amount";
    }
    
    public FilterMonthlySpendingCliCommand(
        CliListAggregator<TransactionMonthTotalAggregate> aggregator,
        string filterOn) 
        : base(filterOn)
    {
        Aggregator = aggregator;
    }
    
    public CliListAggregator<TransactionMonthTotalAggregate> Aggregator { get; }
}