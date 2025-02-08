using System.Text.Json.Serialization;

namespace Ynab.Responses.Accounts;

public class AccountResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("balance")
    public int Balance { get; set; }
    
    [JsonPropertyName("on_budget")]
    public bool OnBudget { get; set; }
    
    public bool Closed {get; set; }
}