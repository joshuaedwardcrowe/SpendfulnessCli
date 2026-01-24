using System.ComponentModel.DataAnnotations;

namespace Spendfulness.Database.Sqlite.Accounts;

public class CustomAccountType
{
    public int Id { get; set; }

    [MaxLength(2000)]
    public required string Name { get; set; }
}

public static class CustomAccountTypes
{
    public const string AmericanExpressRewards = "American Express Rewards";
}