using YnabCli.Database.Users;

namespace YnabCli.Database.Commitments;

public class Commitment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public CommitmentType Type { get; set; }
    public User User { get; set; }
}