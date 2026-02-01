using KitCli.Abstractions.Aggregators.Filters;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;
using KitCli.Commands.Abstractions.Outcomes.Reusable.Page;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using YnabSharp.Extensions;

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