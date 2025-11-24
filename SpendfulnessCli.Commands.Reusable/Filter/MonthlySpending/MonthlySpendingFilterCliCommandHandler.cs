using Cli.Commands.Abstractions.Filters;
using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public class MonthlySpendingFilterCliCommandHandler : CliCommandHandler, ICliCommandHandler<MonthlySpendingFilterCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(MonthlySpendingFilterCliCommand command, CancellationToken cancellationToken)
    {
        var appliedFilters = command.FilterOn switch
        {
            MonthlySpendingFilterCliCommand.FilterNames.TotalAmount
                => ApplyTotalAmountFilter(command),
            
            // TODO: Do better than this.
            _ => throw new ArgumentOutOfRangeException()
        };

        return AsyncOutcomeAs(appliedFilters.ToList());
    }
    
    private static IEnumerable<AppliedFilter> ApplyTotalAmountFilter(MonthlySpendingFilterCliCommand command)
    {
        if (!command.GreaterThan.HasValue) yield break;
        
        command
            .Aggregator
            .AfterAggregation(aggregates =>
                aggregates.Where(aggregate => aggregate.TotalAmount >= command.GreaterThan));
        
        yield return new ValuedAppliedFilter<decimal>(
            MonthlySpendingFilterCliCommand.FilterNames.TotalAmount,
            nameof(MonthlySpendingFilterCliCommand.GreaterThan),
            command.GreaterThan.Value);
    }
}