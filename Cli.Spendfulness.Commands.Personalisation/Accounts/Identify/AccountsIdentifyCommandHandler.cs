using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Database;
using Cli.Spendfulness.Database.Accounts;
using Ynab.Extensions;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;

public class AccountsIdentifyCommandHandler : CommandHandler, ICommandHandler<AccountsIdentifyCommand>
{
    private readonly YnabCliDb _db;
    private readonly ConfiguredBudgetClient _configuredBudgetClient;

    public AccountsIdentifyCommandHandler(YnabCliDb db, ConfiguredBudgetClient configuredBudgetClient)
    {
        _db = db;
        _configuredBudgetClient = configuredBudgetClient;
    }

    public async Task<CliCommandOutcome> Handle(AccountsIdentifyCommand command, CancellationToken cancellationToken)
    {
        var user = await _db.GetActiveUser();
        var accountTypes = await _db.GetAccountTypes();
        
        var budget = await _configuredBudgetClient.GetDefaultBudget();
        var accounts = await budget.GetAccounts();

        var account = accounts.Find(command.YnabAccountName);
        if (account == null)
        {
            throw new YnabCliDbException(
                YnabCliDbExceptionCode.DataNotFound,
                "Account not found");
        }
        
        var type = accountTypes.Find(command.CustomAccountTypeName);
        if (type == null)
        {
            throw new YnabCliDbException(
                YnabCliDbExceptionCode.DataNotFound,
                "Name of a custom account type not found");
        }

        var accountAccountType = user.AccountAttributes.Find(account.Id);
        if (accountAccountType != null)
        {
            accountAccountType.CustomAccountType = type;
        }

        var newAccountAccountType = new AccountAttributes
        {
            YnabAccountId = account.Id,
            CustomAccountType = type,
            User = user,
        };
        
        user.AccountAttributes.Add(newAccountAccountType);
        
        await _db.Save();
        
        return Compile($"Account {account.Name} identified as {type.Name}.");
    }
}