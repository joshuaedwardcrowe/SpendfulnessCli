using Spendfulness.Database.Accounts;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;

public interface IAccountAttributeChangeStrategy
{
    bool AttributeHasChangedSince(AccountsIdentifyCliCommand command, CustomAccountAttributes accountAttributes);
    
    Task<AccountAttributeChange> ChangeAttribute(AccountsIdentifyCliCommand command, CustomAccountAttributes attributes);
}