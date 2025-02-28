using System.Text.Json.Serialization;

namespace Ynab.Responses.Accounts;

public record AccountResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; }
    
    [JsonPropertyName("type")]
    public AccountType Type { get; set; }
    
    [JsonPropertyName("cleared_balance")]
    public required int ClearedBalance { get; set; }
}