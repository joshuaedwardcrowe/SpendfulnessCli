using Ynab.Responses.Transactions;
using Ynab.Sanitisers;

namespace Ynab;

public class SubTransaction(SubTransactionResponse subTransactionResponse)
{
    public string Id => subTransactionResponse.Id;
    public string? Memo => subTransactionResponse.Memo;
    public decimal Amount => MilliunitSanitiser.Calculate(subTransactionResponse.Amount);
    public string PayeeName => subTransactionResponse.PayeeName;
    public Guid? CategoryId => subTransactionResponse.CategoryId;
    public string CategoryName => subTransactionResponse.CategoryName;
    public bool IsTransfer => !string.IsNullOrEmpty(subTransactionResponse.TransferTransactionId);
    
    public bool InCategories(IEnumerable<Guid> categoryIds)
        => CategoryId.HasValue && categoryIds.Contains(CategoryId.Value);
}