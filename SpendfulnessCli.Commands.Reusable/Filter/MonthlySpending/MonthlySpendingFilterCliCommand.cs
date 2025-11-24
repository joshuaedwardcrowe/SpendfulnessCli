using Cli.Abstractions.Aggregators;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public record MonthlySpendingFilterCliCommand : FilterCliCommand
{
    public static class ArgumentNames
    {
        public const string GreaterThan = "gt";
    }

    public static class FilterNames
    {
        public const string TotalAmount = "Total Amount";
    }
    
    public MonthlySpendingFilterCliCommand(
        CliListAggregator<TransactionMonthTotalAggregate> aggregator,
        string filterOn,
        decimal? greaterThan) : base(filterOn)
    {
        Aggregator = aggregator;
        GreaterThan = greaterThan;
    }
    
    public CliListAggregator<TransactionMonthTotalAggregate> Aggregator { get; }
    
    public decimal? GreaterThan { get; }
}