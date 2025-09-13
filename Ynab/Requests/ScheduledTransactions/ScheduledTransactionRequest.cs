using System.Text.Json.Serialization;

namespace Ynab.Requests.ScheduledTransactions;

public class ScheduledTransactionRequest
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("account_id")]
    public Guid AccountId { get; set; }
}