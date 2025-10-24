using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator.ListAggregators;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Cli.Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Spendfulness.Commands.Personalisation.Transactions.List;

public class TransactionListCommandHandler(ConfiguredBudgetClient configuredBudgetClient)
    : CommandHandler, ICommandHandler<TransactionsListCommand>
{
    public async Task<CliCommandOutcome> Handle(TransactionsListCommand command, CancellationToken cancellationToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionYnabAggregator(transactions);

        if (command.PayeeName is not null)
        {
            aggregator.BeforeAggregation(transaction
                => transaction.FilterToPayeeNames([command.PayeeName]));
        }
        
        aggregator.AfterAggregation(aggregates
            => aggregates.OrderByDescending(x => x.Occured));
        
        var viewModel = new TransactionsCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}