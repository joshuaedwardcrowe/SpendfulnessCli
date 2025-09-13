using ConsoleTables;
using Ynab;
using Ynab.Exceptions;
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
            throw new YnabException(
                YnabExceptionCode.CloseAccountCannotBeMovedOnBudget,
                $"{originalAccount.Name} is closed, so cannot be moved on budget.");
        }

        if (originalAccount.OnBudget)
        {
            throw new YnabException(
                YnabExceptionCode.OnBudgetAccountCannotBeMovedOnBudget,
                $"{originalAccount.Name} is already on budget, so cannot be moved on budget.");
        }

        var newAccount = new NewAccount($"[YnabCli Moved: {originalAccount.Name}]", AccountType.Checking, 0);

        var createdAccount =  await budget.CreateAccount(newAccount);
        
        await budget.MoveAccountTransactions(originalAccount, createdAccount);

        return CompileMessage($"Copied Account: {originalAccount.Name} On Budget");
    }
}