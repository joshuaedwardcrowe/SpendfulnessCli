using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Cli.CliTables.ViewModelBuilders;
using Spendfulness.Database;
using Ynab.Extensions;

namespace Cli.Spendfulness.Commands.Personalisation.Transactions.List;

public class TransactionListCliCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<TransactionsListCliCommand>
{
    public async Task<CliCommandOutcome> Handle(TransactionsListCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var budget = await spendfulnessBudgetClient.GetDefaultBudget();

        var transactions = await budget.GetTransactions();

        var aggregator = new TransactionYnabAggregator(transactions);

        if (cliCommand.PayeeName is not null)
        {
            aggregator.BeforeAggregation(transaction
                => transaction.FilterToPayeeNames([cliCommand.PayeeName]));
        }
        
        aggregator.AfterAggregation(aggregates
            => aggregates.OrderByDescending(x => x.Occured));
        
        var viewModel = new TransactionsCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}