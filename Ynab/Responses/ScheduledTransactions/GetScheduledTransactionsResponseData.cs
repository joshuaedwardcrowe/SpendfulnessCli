using System.Text.Json.Serialization;

namespace Ynab.Responses.ScheduledTransactions;

public class GetScheduledTransactionsResponseData
{
    [JsonPropertyName("scheduled_transactions")]
    public required IEnumerable<ScheduledTransactionsResponse> ScheduledTransactions { get; set; }
}

public class GetScheduledTransactionResponseData
{
    [JsonPropertyName("scheduled_transaction")]
    public ScheduledTransactionsResponse ScheduledTransaction { get; set; }
}