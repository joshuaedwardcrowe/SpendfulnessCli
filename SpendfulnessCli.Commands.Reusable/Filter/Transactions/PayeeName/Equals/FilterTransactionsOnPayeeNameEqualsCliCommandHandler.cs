using Cli.Abstractions.Aggregators.Filters;
using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;
using Cli.Commands.Abstractions.Outcomes.Reusable.Page;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using Ynab.Extensions;

namespace SpendfulnessCli.Commands.Reusable.Filter.Transactions.PayeeName.Equals;

public class FilterTransactionsOnPayeeNameEqualsCliCommandHandler 
    : CliCommandHandler, ICliCommandHandler<FilterTransactionsOnPayeeNameEqualsCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(FilterTransactionsOnPayeeNameEqualsCliCommand command, CancellationToken cancellationToken)
    {
        command
            .Aggregator
            .AfterAggregation(aggregates => aggregates.FilterToPayeeNames(command.PayeeName));

        var filter = new ValuedCliListAggregatorFilter<string>(
            FilterTransactionsCliCommand.FilterNames.PayeeName,
            nameof(FilterTransactionsOnPayeeNameEqualsCliCommand.PayeeName),
            command.PayeeName);
        
        var viewModel = new TransactionsCliTableBuilder()
            .WithAggregator(command.Aggregator)
            .WithRowCount(false)
            .Build();

        var outcomes = new CliCommandOutcome[]
        {
            new CliCommandTableOutcome(viewModel),
            new PageSizeCliCommandOutcome(command.Aggregator.PageSize),
            new PageNumberCliCommandOutcome(command.Aggregator.PageNumber),
            new FilterCliCommandOutcome(filter)
        };
        
        return Task.FromResult(outcomes);
    }
}