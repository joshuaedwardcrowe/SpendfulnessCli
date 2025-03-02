using YnabCli.Database.Users;

namespace YnabCli.Database.Accounts;

public class AccountAttributes
{
    public int Id { get; set; }
    public Guid YnabAccountId { get; set; }
    public required CustomAccountType CustomAccountType { get; set; }
    public decimal InterestRate { get; set; }
    public required User User { get; set; } 
}