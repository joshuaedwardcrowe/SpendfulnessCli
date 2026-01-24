using Spendfulness.Database.Sqlite.Accounts;

namespace SpendfulnessCli.Commands.Personalisation.Accounts.Attribute.ChangeStrategies;

public class InterestRateChangeStrategy : IAccountAttributeChangeStrategy
{
    public bool AttributeHasChangedSince(AccountAttributeCliCommand command, CustomAccountAttributes accountAttributes)
        => command.InterestRate.HasValue &&
           command.InterestRate != accountAttributes.InterestRate;

    public Task<AccountAttributeChange> ChangeAttribute(AccountAttributeCliCommand command, CustomAccountAttributes attributes)
    {
        var to = command.InterestRate.ToString();

        attributes.InterestRate = command.InterestRate!.Value;

        var change = new AccountAttributeChange(
            nameof(CustomAccountAttributes.InterestRate),
            string.Empty,
            to!);

        return Task.FromResult(change);
    }
}