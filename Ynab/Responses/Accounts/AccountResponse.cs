using System.Text.Json.Serialization;

namespace Ynab.Responses.Accounts;

public record AccountResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("balance")]
    public required int Balance { get; set; }
    
    [JsonPropertyName("cleared_balance")]
    public required int ClearedBalance { get; set; }
    
    [JsonPropertyName("on_budget")]
    public required bool OnBudget { get; set; }
    
    [JsonPropertyName("closed")]
    public required bool Closed { get; set; }
}