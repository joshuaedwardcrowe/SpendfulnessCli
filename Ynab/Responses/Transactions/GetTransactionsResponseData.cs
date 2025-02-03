namespace Ynab.Responses.Transactions;

public class GetTransactionsResponseData
{
    public required IEnumerable<TransactionResponse> Transactions { get; set; }
}