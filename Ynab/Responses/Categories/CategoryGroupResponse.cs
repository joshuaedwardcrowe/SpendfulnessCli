using System.Text.Json.Serialization;
using Ynab.Responses.Category;
namespace Ynab.Responses.Categories;

public class CategoryGroupResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("categories")]
    public required IEnumerable<CategoryResponse> Categories { get; set; }
}