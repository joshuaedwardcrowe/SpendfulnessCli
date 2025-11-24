using Cli.Commands.Abstractions.Filters;
using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public class FilterMonthlySpendingOnTotalAmountCliCommandHandler 
    : CliCommandHandler, ICliCommandHandler<FilterMonthlySpendingOnTotalAmountCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(FilterMonthlySpendingOnTotalAmountCliCommand command, CancellationToken cancellationToken)
    {
        command
            .Aggregator
            .AfterAggregation(aggregates =>
                aggregates.Where(aggregate => aggregate.TotalAmount >= command.GreaterThan));
        
        
        var appliedFilters = new List<AppliedFilter>
        {
            new ValuedAppliedFilter<decimal>(
                FilterMonthlySpendingCliCommand.FilterNames.TotalAmount,
                nameof(FilterMonthlySpendingOnTotalAmountCliCommand.GreaterThan),
                command.GreaterThan!.Value)
        };
        
        return AsyncOutcomeAs(appliedFilters);
    }
}