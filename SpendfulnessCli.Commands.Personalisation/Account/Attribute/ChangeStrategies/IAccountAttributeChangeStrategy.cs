using Spendfulness.Database.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Account.Attribute.ChangeStrategies;

public interface IAccountAttributeChangeStrategy
{
    bool AttributeHasChangedSince(AccountAttributeCliCommand command, CustomAccountAttributes accountAttributes);
    
    Task<AccountAttributeChange> ChangeAttribute(AccountAttributeCliCommand command, CustomAccountAttributes attributes);
}