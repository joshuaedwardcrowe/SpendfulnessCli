namespace Cli.Spendfulness.Database.Accounts;

public static class  AccountAttributesExtensions
{
    public static AccountAttributes? Find(this ICollection<AccountAttributes> accountTypes, Guid id)
        => accountTypes.FirstOrDefault(accountType => accountType.YnabAccountId == id);
}