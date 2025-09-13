using System.Text.Json.Serialization;

namespace Ynab.Responses.Transactions;

public class GetTransactionsResponse
{
    [JsonPropertyName("transactions")]
    public required IEnumerable<TransactionResponse> Transactions { get; set; }
}

public class GetTransactionResponse
{
    [JsonPropertyName("transaction")]
    public required TransactionResponse Transaction { get; set; }
}