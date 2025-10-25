using System.ComponentModel.DataAnnotations;
using Spendfulness.Database.Users;

namespace Spendfulness.Database.Milestones;

public class Milestone
{
    public int Id { get; set; }
    [MaxLength(2000)]
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public int Ranking { get; set; }
    public required User User { get; set; }
}