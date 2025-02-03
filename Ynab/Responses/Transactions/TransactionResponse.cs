using System.Text.Json.Serialization;

namespace Ynab.Responses.Transactions;

public class TransactionResponse
{
    [JsonPropertyName("date")]
    public DateTime Occured { get; set; }
    
    public int Amount { get; set; }
    
    [JsonPropertyName("flag_color")]
    public string? FlagColor { get; set; }
    
    [JsonPropertyName("flag_name")]
    public string? FlagName { get; set; }
    
    [JsonPropertyName("payee_name")]
    public required string PayeeName { get; set; }
    
    [JsonPropertyName("category_id")]
    public Guid? CategoryId { get; set; }
}