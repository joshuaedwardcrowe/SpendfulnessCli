using Spendfulness.Database.Accounts;

namespace Cli.Spendfulness.Commands.Personalisation.Accounts.Identify;

public class InterestRateChangeStrategy : IAccountAttributeChangeStrategy
{
    public bool AttributeHasChangedSince(AccountsIdentifyCliCommand command, CustomAccountAttributes accountAttributes)
        => command.InterestRate.HasValue && command.InterestRate != accountAttributes.InterestRate;

    public Task<AccountAttributeChange> ChangeAttribute(AccountsIdentifyCliCommand command, CustomAccountAttributes attributes)
    {
        throw new NotImplementedException();
    }
}