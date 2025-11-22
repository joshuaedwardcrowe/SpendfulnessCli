using Spendfulness.Database.Accounts;
using SpendfulnessCli.Commands.Personalisation.Accounts.Attribute;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Identify.ChangeStrategies;

public class InterestRateChangeStrategy : IAccountAttributeChangeStrategy
{
    public bool AttributeHasChangedSince(AccountAttributeCliCommand command, CustomAccountAttributes accountAttributes)
        => command.InterestRate.HasValue && command.InterestRate != accountAttributes.InterestRate;

    public Task<AccountAttributeChange> ChangeAttribute(AccountAttributeCliCommand command, CustomAccountAttributes attributes)
    {
        throw new NotImplementedException();
    }
}