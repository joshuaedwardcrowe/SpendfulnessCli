using Spendfulness.Database.Users;

namespace Spendfulness.Database.Accounts;

// TODO: 'Attributes' doesn't respect DB convention. Think of a better name.
public class CustomAccountAttributes
{
    public int Id { get; set; }
    // TODO: These are bound to YNAB, but they don't NEED to be. They could come from Xero, who knows.
    public Guid YnabAccountId { get; set; }
    public string YnabAccountName { get; set; }
    public CustomAccountType? CustomAccountType { get; set; }
    public decimal InterestRate { get; set; }
    public required User User { get; set; } 
}