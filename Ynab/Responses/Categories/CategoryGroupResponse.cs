using Ynab.Responses.Category;
namespace Ynab.Responses.Categories;

public class CategoryGroupResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required IEnumerable<CategoryResponse> Categories { get; set; }
}