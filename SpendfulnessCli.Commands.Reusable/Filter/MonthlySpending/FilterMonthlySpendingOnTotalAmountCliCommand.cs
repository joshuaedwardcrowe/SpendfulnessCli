using Cli.Abstractions.Aggregators;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public record FilterMonthlySpendingOnTotalAmountCliCommand : FilterMonthlySpendingCliCommand
{
    public new static class ArgumentNames
    {
        public const string GreaterThan = "gt";
    }
    
    public FilterMonthlySpendingOnTotalAmountCliCommand(
        CliListAggregator<TransactionMonthTotalAggregate> aggregator,
        string filterOn,
        decimal? greaterThan)  
        : base(aggregator, filterOn)
    {
        GreaterThan = greaterThan;
    }
        
    public decimal? GreaterThan { get; }
}