using ConsoleTables;
using Ynab;
using YnabCli.Commands.Handlers;
using YnabCli.Commands.Organisation.MoveOnBudget;
using YnabCli.Database;

namespace YnabCli.Commands.Organisation.CopyOnBudget;

public class CopyOnBudgetCommandHandler(ConfiguredBudgetClient budgetClient) : CommandHandler, ICommandHandler<CopyOnBudgetCommand>
{
    public async Task<ConsoleTable> Handle(CopyOnBudgetCommand command, CancellationToken cancellationToken)
    {
        var budget = await budgetClient.GetDefaultBudget();

        var originalAccount = await budget.GetAccount(command.AccountId);

        if (originalAccount.Closed)
        {
            // TODO: Migrate to use exceptions consistent with other handlers.
            throw new InvalidOperationException("Cannot move a closed account.");
        }

        if (originalAccount.OnBudget)
        {
            // TODO: Migrate to use exceptions consistent with other handlers.
            throw new InvalidOperationException("Account must be on budget to move.");
        }

        var newAccount = new NewAccount($"[YnabCli Moved: {originalAccount.Name}]", AccountType.Checking, 0);

        var createdAccount =  await budget.CreateAccount(newAccount);
        
        await budget.MoveAllTransactions(originalAccount, createdAccount);

        return CompileMessage($"Copied Account: {originalAccount.Name} On Budget");
    }
}