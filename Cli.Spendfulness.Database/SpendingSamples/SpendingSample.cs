using System.ComponentModel.DataAnnotations;

namespace Cli.Spendfulness.Database.SpendingSamples;

public class SpendingSample
{
    public Guid Id { get; set; }
    
    public DateTime Created { get; set; }

    // Which transaction this Spending Sample was derived from. 
    [MaxLength(50)]
    public string? YnabTransactionId { get; set; }

    public ICollection<SpendingSampleMatchCriteria> Matches { get; set; } = [];
}