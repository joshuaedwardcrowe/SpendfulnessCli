using Spendfulness.Database.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Identify.ChangeStrategies;

public class InterestRateChangeStrategy : IAccountAttributeChangeStrategy
{
    public bool AttributeHasChangedSince(AccountsIdentifyCliCommand command, CustomAccountAttributes accountAttributes)
        => command.InterestRate.HasValue && command.InterestRate != accountAttributes.InterestRate;

    public Task<AccountAttributeChange> ChangeAttribute(AccountsIdentifyCliCommand command, CustomAccountAttributes attributes)
    {
        throw new NotImplementedException();
    }
}