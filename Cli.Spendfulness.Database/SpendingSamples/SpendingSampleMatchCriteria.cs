using System.ComponentModel.DataAnnotations;
using Ynab;

namespace Cli.Spendfulness.Database.SpendingSamples;

// TODO: Not really sure if this can be considered a 'match'.

/// <summary>
/// A sample of unique spending that can be matched on.
///
/// 'Spending' covers an intent to spend somewhere, on something, for some reason.
/// </summary>
public class SpendingSampleMatchCriteria
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
    
    public ICollection<SpendingSampleMatchCriteriaPrice> Prices { get; set; } = [];
    
    public SpendingSampleMatchCriteriaPrice MostRecentPrice => Prices
        .OrderBy(price => price.EffectiveFrom)
        .First();

    // TODO: Move me to a mapper.
    public static SpendingSampleMatchCriteria CreateFrom(SplitTransactions transaction) 
        => new()
        {
            YnabPayeeId = transaction.PayeeId!.Value,
            YnabCategoryId = transaction.CategoryId!.Value,
            YnabMemo = transaction.Memo!,
            Prices = new List<SpendingSampleMatchCriteriaPrice>
            {
                new()
                {
                    EffectiveFrom = DateTime.UtcNow,
                    Amount = transaction.Amount,
                }
            }
        };
}