using System.Text.Json.Serialization;

namespace Ynab.Responses.Accounts;

public record AccountResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }
    
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("type")]
    public required AccountType Type { get; set; }
    
    [JsonPropertyName("cleared_balance")]
    public required int ClearedBalance { get; set; }
}