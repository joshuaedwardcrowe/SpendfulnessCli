using System.ComponentModel.DataAnnotations;
using Cli.Spendfulness.Database.Users;

namespace Cli.Spendfulness.Database.Commitments;

public class Commitment
{
    public int Id { get; set; }
    [MaxLength(2000)]
    public required string Name { get; set; }
    public DateOnly? Started { get; set; }
    public DateOnly? RequiredBy  { get; set; }
    public decimal? Funded { get; set; }
    public decimal? Needed { get; set; }
    public required User User { get; set; }
    public required Guid YnabCategoryId { get; set; }
    
    public decimal? Target => Funded + Needed;
}