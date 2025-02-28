using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Commands.Factories;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.Database.Accounts;

namespace YnabCli.Commands.Personalisation.Accounts.Identify;

public class AccountsIdentifyCommandHandler : CommandHandler, ICommandHandler<AccountsIdentifyCommand>
{
    private readonly UnitOfWork _unitOfWork;
    private readonly CommandBudgetGetter _commandBudgetGetter;

    public AccountsIdentifyCommandHandler(UnitOfWork unitOfWork, CommandBudgetGetter commandBudgetGetter)
    {
        _unitOfWork = unitOfWork;
        _commandBudgetGetter = commandBudgetGetter;
    }

    public async Task<ConsoleTable> Handle(AccountsIdentifyCommand command, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.GetActiveUser();
        var accountTypes = await _unitOfWork.GetAccountTypes();
        
        var budget = await _commandBudgetGetter.Get();
        var accounts = await budget.GetAccounts();

        var account = accounts.Find(command.YnabAccountName);
        if (account == null)
        {
            throw new Exception("Account not found");
        }
        
        var type = accountTypes.Find(command.AccountAccountTypeName);
        if (type == null)
        {
            throw new Exception("Invalid type passed");
        }

        var accountAccountType = user.CustomAccountTypes.Find(account.Id);
        if (accountAccountType != null)
        {
            accountAccountType.CustomAccountType = type;
        }

        var newAccountAccountType = new AccountCustomAccountType
        {
            YnabAccountId = account.Id,
            CustomAccountType = type,
            User = user,
        };
        
        user.CustomAccountTypes.Add(newAccountAccountType);
        
        await _unitOfWork.Save();
        
        return CompileMessage($"Account {account.Name} identified as {type.Name}.");
    }
}