using YnabCli.Database.Users;

namespace YnabCli.Database.Accounts;

public class AccountCustomAccountType
{
    public int Id { get; set; }
    public Guid YnabAccountId { get; set; }
    public CustomAccountType CustomAccountType { get; set; }
    public User User { get; set; } 
}