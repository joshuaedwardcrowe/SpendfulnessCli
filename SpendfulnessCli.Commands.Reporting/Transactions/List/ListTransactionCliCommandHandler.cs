using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;
using Cli.Commands.Abstractions.Outcomes.Reusable.Page;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;
using SpendfulnessCli.CliTables.ViewModelBuilders;
using Ynab.Extensions;

namespace SpendfulnessCli.Commands.Reporting.Transactions.List;

public class ListTransactionCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<ListTransactionCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(ListTransactionCliCommand command, CancellationToken cancellationToken)
    {
        var budget = await spendfulnessBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionPagedListAggregator(
            transactions,
            command.PageNumber,
            command.PageSize);

        // TODO: Move to filter command.
        if (command.PayeeName is not null)
        {
            aggregator.BeforeAggregation(transaction
                => transaction.FilterToPayeeNames([command.PayeeName]));
        }
        
        // TODO: Move to filter command.
        aggregator.AfterAggregation(aggregates
            => aggregates.OrderByDescending(x => x.Occured));
        
        var viewModel = new TransactionsCliTableBuilder()
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return
        [
            new CliCommandTableOutcome(viewModel),
            new PageSizeCliCommandOutcome(command.PageSize),
            new PageNumberCliCommandOutcome(command.PageNumber)
        ];
    }
}