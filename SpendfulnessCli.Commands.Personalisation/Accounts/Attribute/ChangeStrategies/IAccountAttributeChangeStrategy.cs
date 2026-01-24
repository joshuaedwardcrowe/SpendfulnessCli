using Spendfulness.Database.Sqlite.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Attribute.ChangeStrategies;

public interface IAccountAttributeChangeStrategy
{
    bool AttributeHasChangedSince(AccountAttributeCliCommand command, CustomAccountAttributes accountAttributes);
    
    Task<AccountAttributeChange> ChangeAttribute(AccountAttributeCliCommand command, CustomAccountAttributes attributes);
}