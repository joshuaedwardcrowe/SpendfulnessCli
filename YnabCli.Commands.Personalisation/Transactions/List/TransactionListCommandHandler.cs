using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Personalisation.Transactions.List;

public class TransactionListCommandHandler(ConfiguredBudgetClient configuredBudgetClient)
    : CommandHandler, ICommandHandler<TransactionsListCommand>
{
    public async Task<ConsoleTable> Handle(TransactionsListCommand command, CancellationToken cancellationToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionAggregator(transactions);

        if (command.PayeeName is not null)
        {
            aggregator.BeforeAggregation(transaction
                => transaction.FilterToPayeeNames([command.PayeeName]));
        }
        
        aggregator.AfterAggregation(aggregates
            => aggregates.OrderByDescending(x => x.Occured));
        
        var viewModel = new TransactionsViewModelBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}