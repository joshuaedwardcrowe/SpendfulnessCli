using Spendfulness.Database.Users;

namespace Spendfulness.Database.Accounts;

public class CustomAccountAttributes
{
    public int Id { get; set; }
    public Guid YnabAccountId { get; set; }
    public required CustomAccountType CustomAccountType { get; set; }
    public decimal InterestRate { get; set; }
    public required User User { get; set; } 
}