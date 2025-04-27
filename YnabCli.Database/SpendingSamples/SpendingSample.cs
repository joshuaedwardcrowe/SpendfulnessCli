namespace YnabCli.Database.SpendingSamples;

/// <summary>
/// A sample of unique spending.
///
/// 'Spending' covers an intent to spend somewhere, on something, for some reason.
/// </summary>
public class SpendingSample
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
    public string YnabMemo { get; set; }
    
    
    public ICollection<SpendingSamplePrices> Prices { get; set; } = new List<SpendingSamplePrices>();
}