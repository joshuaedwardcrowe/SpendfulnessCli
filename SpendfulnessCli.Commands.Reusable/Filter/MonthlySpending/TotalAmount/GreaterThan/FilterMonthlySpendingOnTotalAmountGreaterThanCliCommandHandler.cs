using Cli.Commands.Abstractions.Filters;
using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending.TotalAmount.GreaterThan;

public class FilterMonthlySpendingOnTotalAmountGreaterThanCliCommandHandler 
    : CliCommandHandler, ICliCommandHandler<FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand command, CancellationToken cancellationToken)
    {
        command
            .Aggregator
            .AfterAggregation(aggregates =>
                aggregates.Where(aggregate => aggregate.TotalAmount >= command.GreaterThan));
        
        
        var appliedFilters = new List<AppliedFilter>
        {
            new ValuedAppliedFilter<decimal>(
                FilterMonthlySpendingCliCommand.FilterNames.TotalAmount,
                nameof(FilterMonthlySpendingOnTotalAmountGreaterThanCliCommand.GreaterThan),
                command.GreaterThan!.Value)
        };
        
        return AsyncOutcomeAs(appliedFilters);
    }
}