using System.Text.Json.Serialization;

namespace Ynab.Responses.Transactions;

public class GetTransactionsResponseData
{
    [JsonPropertyName("transactions")]
    public required IEnumerable<TransactionResponse> Transactions { get; set; }
}

public class GetTransactionResponseData
{
    [JsonPropertyName("transaction")]
    public required TransactionResponse Transaction { get; set; }
}