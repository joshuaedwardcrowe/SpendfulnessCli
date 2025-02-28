namespace YnabCli.Database.Accounts;

public static class AccountCustomAccountTypeExtensions
{
    public static AccountCustomAccountType? Find(this ICollection<AccountCustomAccountType> accountTypes, Guid id)
        => accountTypes.FirstOrDefault(accountType => accountType.YnabAccountId == id);
}