namespace Cli.Spendfulness.Database.Accounts;

public static class  CustomAccountAttributesExtensions
{
    public static CustomAccountAttributes? Find(this ICollection<CustomAccountAttributes> accountTypes, Guid id)
        => accountTypes.FirstOrDefault(accountType => accountType.YnabAccountId == id);
}