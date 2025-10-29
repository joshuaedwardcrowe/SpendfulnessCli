using Spendfulness.Database.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Identify.ChangeStrategies;

public interface IAccountAttributeChangeStrategy
{
    bool AttributeHasChangedSince(AccountsIdentifyCliCommand command, CustomAccountAttributes accountAttributes);
    
    Task<AccountAttributeChange> ChangeAttribute(AccountsIdentifyCliCommand command, CustomAccountAttributes attributes);
}