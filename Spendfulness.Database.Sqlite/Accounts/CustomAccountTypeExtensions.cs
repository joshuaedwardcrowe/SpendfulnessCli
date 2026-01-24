namespace Spendfulness.Database.Sqlite.Accounts;

public static class CustomAccountTypeExtensions
{
    public static CustomAccountType? Find(this ICollection<CustomAccountType> customAccountTypes, string name)
        => customAccountTypes.FirstOrDefault(customAccountType => customAccountType.Name == name);
}