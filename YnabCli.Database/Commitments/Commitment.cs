using YnabCli.Database.Users;

namespace YnabCli.Database.Commitments;

public class Commitment
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public required CommitmentType Type { get; set; }
    public required User User { get; set; }
}