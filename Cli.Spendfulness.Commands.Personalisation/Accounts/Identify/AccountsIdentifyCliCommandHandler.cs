using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using Spendfulness.Database.Abstractions;
using Spendfulness.Database.Accounts;
using Ynab.Extensions;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;

public class AccountsIdentifyCliCommandHandler : CliCommandHandler, ICliCommandHandler<AccountsIdentifyCliCommand>
{
    private readonly SpendfulnessDb _db;
    private readonly SpendfulnessBudgetClient _budget;

    public AccountsIdentifyCliCommandHandler(SpendfulnessDb db, SpendfulnessBudgetClient budget)
    {
        _db = db;
        _budget = budget;
    }

    public async Task<CliCommandOutcome> Handle(AccountsIdentifyCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var user = await _db.GetActiveUser();
        var accountTypes = await _db.GetAccountTypes();
        
        var budget = await _budget.GetDefaultBudget();
        var accounts = await budget.GetAccounts();

        var account = accounts.Find(cliCommand.YnabAccountName);
        if (account == null)
        {
            throw new SpendfulnessDbException(
                SpendfulnessDbExceptionCode.EntityNotFound,
                "Account not found");
        }
        
        var type = accountTypes.Find(cliCommand.CustomAccountTypeName);
        if (type == null)
        {
            throw new SpendfulnessDbException(
                SpendfulnessDbExceptionCode.EntityNotFound,
                "Name of a custom account type not found");
        }

        var accountAccountType = user.AccountAttributes.Find(account.Id);
        if (accountAccountType != null)
        {
            accountAccountType.CustomAccountType = type;
        }

        var newAccountAccountType = new CustomAccountAttributes
        {
            YnabAccountId = account.Id,
            CustomAccountType = type,
            User = user,
        };
        
        user.AccountAttributes.Add(newAccountAccountType);
        
        await _db.Save();
        
        // TODO: Maybe make this a table of the account's cusotm properties.
        return Compile($"Account {account.Name} identified as {type.Name}.");
    }
}