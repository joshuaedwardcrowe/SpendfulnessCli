using Spendfulness.Database.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Account.Attribute.ChangeStrategies;

public class InterestRateChangeStrategy : IAccountAttributeChangeStrategy
{
    public bool AttributeHasChangedSince(AccountAttributeCliCommand command, CustomAccountAttributes accountAttributes)
        => command.InterestRate.HasValue && command.InterestRate != accountAttributes.InterestRate;

    public Task<AccountAttributeChange> ChangeAttribute(AccountAttributeCliCommand command, CustomAccountAttributes attributes)
    {
        // TODO: Do something with this.
        throw new NotImplementedException();
    }
}