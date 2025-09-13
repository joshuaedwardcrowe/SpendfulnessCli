using Ynab.Responses.Transactions;
using Ynab.Sanitisers;

namespace Ynab;

// TODO: Hate that this is plural...
public class SplitTransactions(SplitTransactionResponse splitTransactionResponse)
{
    public string Id => splitTransactionResponse.Id;
    public string? Memo => splitTransactionResponse.Memo;
    public decimal Amount => MilliunitSanitiser.Calculate(splitTransactionResponse.Amount);
    public Guid? PayeeId => splitTransactionResponse.PayeeId;
    public string PayeeName => splitTransactionResponse.PayeeName;
    public Guid? CategoryId => splitTransactionResponse.CategoryId;
    public string CategoryName => splitTransactionResponse.CategoryName;
    public bool IsTransfer => !string.IsNullOrEmpty(splitTransactionResponse.TransferTransactionId);
    public Guid? AccountId => splitTransactionResponse.AccountId;
    
    public bool InCategories(IEnumerable<Guid> categoryIds)
        => CategoryId.HasValue && categoryIds.Contains(CategoryId.Value);
    
    public bool IsFullyFormed => this is { PayeeId: not null, CategoryId: not null, Memo: not null };
}