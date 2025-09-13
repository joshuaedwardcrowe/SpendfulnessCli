using ConsoleTables;
using Ynab;
using YnabCli.Commands.Handlers;
using YnabCli.Database;

namespace YnabCli.Commands.Organisation.MoveOnBudget;

public class MoveOnBudgetCommandHandler(ConfiguredBudgetClient budgetClient) : ICommandHandler<MoveOnBudgetCommand>
{
    public async Task<ConsoleTable> Handle(MoveOnBudgetCommand command, CancellationToken cancellationToken)
    {
        var budget = await budgetClient.GetDefaultBudget();

        var accountToMove = await budget.GetAccount(command.AccountId);

        ValidateAccountCanBeMoved(accountToMove);
        
        var newAccount = new NewAccount("Placeholder", AccountType.Checking, 0);

        var createdAccount =  await budget.CreateAccount(newAccount);
        
        var transactions = await accountToMove.GetTransactions();
        //
        // if (!transactions.Any())
        // {
        //     // TODO: Migrate to use exceptions consistent with other handlers.
        //     throw new InvalidOperationException("Cannot move an account with no transactions.");
        // }
        //
        // foreach (var transaction in transactions)
        // {
        //     
        // }
        
        // TODO: Migrate transactions.
        // TODO: Delete old transactions.
        // TODO: Delete off budget account.

        return new ConsoleTable("Status", "End of move On budget");
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