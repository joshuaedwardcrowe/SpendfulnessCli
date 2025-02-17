using System.Text.Json.Serialization;

namespace Ynab.Responses.Transactions;

public class GetTransactionsResponseData
{
    [JsonPropertyName("transactions")]
    public required IEnumerable<TransactionResponse> Transactions { get; set; }
}