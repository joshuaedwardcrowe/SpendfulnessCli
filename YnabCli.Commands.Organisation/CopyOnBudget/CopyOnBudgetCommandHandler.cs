using ConsoleTables;
using Ynab;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Organisation.MoveOnBudget;

public class CopyOnBudgetCommandHandler(ConfiguredBudgetClient budgetClient) : CommandHandler, ICommandHandler<CopyOnBudgetCommand>
{
    public async Task<ConsoleTable> Handle(CopyOnBudgetCommand command, CancellationToken cancellationToken)
    {
        var budget = await budgetClient.GetDefaultBudget();

        var accountToMove = await budget.GetAccount(command.AccountId);

        ValidateAccountCanBeMoved(accountToMove);
        
        // TODO: Use old account name.
        var newAccount = new NewAccount($"[Moved Account: {accountToMove.Name}]", AccountType.Checking, 0);

        var createdAccount =  await budget.CreateAccount(newAccount);
        
        var transactions = await accountToMove.GetTransactions();

        var transactionsToMove = transactions
            .Where(t => t.PayeeName != AutomatedPayeeNames.StartingBalance)
            .Select(t => new MovedTransaction
            {
                Id = t.Id,
                AccountId = createdAccount.Id,
            })
            .ToList();
        
        var movedTransactions = await budget.MoveTransactions(transactionsToMove);
        
        var aggregator = new TransactionAggregator(movedTransactions);
        
        var viewModel = new TransactionsViewModelBuilder()
            .WithAggregator(aggregator)
            .Build();
        
        return Compile(viewModel);
    }

    private void ValidateAccountCanBeMoved(Account account)
    {
        if (account.Closed)
        {
            // TODO: Migrate to use exceptions consistent with other handlers.
            throw new InvalidOperationException("Cannot move a closed account.");
        }

        if (account.OnBudget)
        {
            // TODO: Migrate to use exceptions consistent with other handlers.
            throw new InvalidOperationException("Account must be on budget to move.");
        }
    }
}