using System.ComponentModel.DataAnnotations;

namespace YnabCli.Database.SpendingSamples;

public class SpendingSample
{
    public Guid Id { get; set; }
    
    public DateTime Created { get; set; }

    [MaxLength(50)]
    public required string YnabTransactionDerivedFromId { get; set; }

    public ICollection<SpendingSampleMatch> Matches { get; set; } = [];
}