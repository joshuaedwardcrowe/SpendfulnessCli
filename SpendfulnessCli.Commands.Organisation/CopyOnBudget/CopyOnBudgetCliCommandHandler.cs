using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using Ynab;
using Ynab.Exceptions;

namespace SpendfulnessCli.Commands.Organisation.CopyOnBudget;

public class CopyOnBudgetCliCommandHandler(SpendfulnessBudgetClient budgetClient) : CliCommandHandler, ICliCommandHandler<CopyOnBudgetCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(CopyOnBudgetCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var budget = await budgetClient.GetDefaultBudget();

        var originalAccount = await budget.GetAccount(cliCommand.AccountId);

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

        return OutcomeAs($"Copied Account: {originalAccount.Name} On Budget");
    }
}