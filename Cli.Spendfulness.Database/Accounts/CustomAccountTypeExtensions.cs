namespace Cli.Spendfulness.Database.Accounts;

public static class CustomAccountTypeExtensions
{
    public static CustomAccountType? Find(this ICollection<CustomAccountType> customAccountTypes, string name)
        => customAccountTypes.FirstOrDefault(customAccountType => customAccountType.Name == name);
}