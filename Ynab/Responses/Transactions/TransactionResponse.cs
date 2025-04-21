using System.Text.Json.Serialization;

namespace Ynab.Responses.Transactions;

public class TransactionResponse : SubTransactionResponse
{
    [JsonPropertyName("date")]
    public DateTime Occured { get; set; }
    
    [JsonPropertyName("flag_color")]
    public FlagColor? FlagColor { get; set; }
    
    [JsonPropertyName("flag_name")]
    public string? FlagName { get; set; }
    
    [JsonPropertyName("subtransactions")]
    public IEnumerable<SubTransactionResponse> SubTransactions { get; set; } = [];
}