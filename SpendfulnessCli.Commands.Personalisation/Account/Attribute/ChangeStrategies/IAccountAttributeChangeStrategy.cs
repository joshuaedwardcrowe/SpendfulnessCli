using Spendfulness.Database.Accounts;
using SpendfulnessCli.Commands.Personalisation.Accounts.Attribute;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Identify.ChangeStrategies;

public interface IAccountAttributeChangeStrategy
{
    bool AttributeHasChangedSince(AccountAttributeCliCommand command, CustomAccountAttributes accountAttributes);
    
    Task<AccountAttributeChange> ChangeAttribute(AccountAttributeCliCommand command, CustomAccountAttributes attributes);
}