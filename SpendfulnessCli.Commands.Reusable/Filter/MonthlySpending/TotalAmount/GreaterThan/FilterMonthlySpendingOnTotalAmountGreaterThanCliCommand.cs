using Cli.Abstractions.Aggregators;
using Spendfulness.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending.TotalAmount.GreaterThan;

public record FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand : FilterMonthlySpendingCliCommand
{
    public new static class ArgumentNames
    {
        public const string GreaterThan = "gt";
    }
    
    public FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand(
        CliListAggregator<TransactionMonthTotalAggregate> aggregator,
        string filterOn,
        decimal? greaterThan)  
        : base(aggregator, filterOn)
    {
        GreaterThan = greaterThan;
    }
        
    public decimal? GreaterThan { get; }
}