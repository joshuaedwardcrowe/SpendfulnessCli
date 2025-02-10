using System.Text.Json.Serialization;

namespace Ynab.Responses.Category;

public class CategoryResponse
{
    public Guid Id { get; set; }

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