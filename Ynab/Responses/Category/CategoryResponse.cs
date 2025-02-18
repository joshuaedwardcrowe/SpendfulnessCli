using System.Text.Json.Serialization;

namespace Ynab.Responses.Category;

public class CategoryResponse
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Money in this category assigned.
    /// </summary>
    public int Assigned => Budgeted;
    
    [JsonPropertyName("budgeted")]
    public int Budgeted { get; set; }

    /// <summary>
    /// Money in this category available to spend.
    /// </summary>
    public int Available => Balance;
    
    [JsonPropertyName("balance")]
    public int Balance { get; set; }
}