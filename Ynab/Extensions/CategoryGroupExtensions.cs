namespace Ynab.Extensions;

public static class CategoryGroupExtensions
{
    public static IEnumerable<CategoryGroup> FilterToSpendingCategories(
        this IEnumerable<CategoryGroup> categoryGroups)
         => categoryGroups.Where(categoryGroup => !categoryGroup.Name.Contains("Farm"));
    
    public static IEnumerable<Guid> GetCategoryIds(
        this CategoryGroup categoryGroup)
            => categoryGroup.Categories.Select(category => category.Id);
}