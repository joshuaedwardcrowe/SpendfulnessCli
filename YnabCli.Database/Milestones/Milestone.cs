using YnabCli.Database.Users;

namespace YnabCli.Database.Milestones;

public class Milestone
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Amount { get; set; }
    public int Ranking { get; set; }
    public User User { get; set; }
}