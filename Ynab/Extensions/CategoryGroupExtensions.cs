namespace Ynab.Extensions;

public static class CategoryGroupExtensions
{
    public static IEnumerable<CategoryGroup> FilterToSpendingCategories(
        this IEnumerable<CategoryGroup> categoryGroups)
            => categoryGroups.Where(cg => !cg.Name.Contains("Farm"));
    
    public static IEnumerable<CategoryGroup> FilterTo(
        this IEnumerable<CategoryGroup> categoryGroups, params string[] categoryGroupNames)
            => categoryGroups.Where(cg => categoryGroupNames.Contains(cg.Name));
    
    public static IEnumerable<Guid> GetCategoryIds(
        this CategoryGroup categoryGroup)
            => categoryGroup.Categories.Select(category => category.Id);
}