using System.ComponentModel.DataAnnotations;

namespace YnabCli.Database.SpendingSamples;

// TODO: Not really sure if this can be considered a 'match'.

/// <summary>
/// A sample of unique spending.
///
/// 'Spending' covers an intent to spend somewhere, on something, for some reason.
/// </summary>
public class SpendingSampleMatch
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Who the spending happened with.
    /// </summary>
    public Guid YnabPayeeId { get; set; }
    
    /// <summary>
    /// Why the spending happened.
    /// </summary>
    public Guid YnabCategoryId { get; set; }
    
    /// <summary>
    /// What the spending happened on.
    /// </summary>
    [MaxLength(1000)]
    public required string YnabMemo { get; set; }
    
    public ICollection<SpendingSampleMatchPrice> Prices { get; set; } = [];
    
    public SpendingSampleMatchPrice MostRecentPrice => Prices
        .OrderBy(price => price.EffectiveFrom)
        .First();
}