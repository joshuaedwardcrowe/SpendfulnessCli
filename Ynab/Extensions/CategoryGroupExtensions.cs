using Ynab.Connected;

namespace Ynab.Extensions;

public static class CategoryGroupExtensions
{
    public static IEnumerable<CategoryGroup> FilterTo(
        this IEnumerable<CategoryGroup> categoryGroups, params string[] categoryGroupNames)
        => categoryGroups.Where(cg => categoryGroupNames.Contains(cg.Name));
    
    public static IEnumerable<CategoryGroup> FilterTo(
        this IEnumerable<CategoryGroup> categoryGroups, IEnumerable<string> categoryGroupNames)
            => categoryGroups.FilterTo(categoryGroupNames.ToArray());
    
    public static IEnumerable<CategoryGroup> FilterToSpendingCategories(
        this IEnumerable<CategoryGroup> categoryGroups)
            => categoryGroups.Where(cg => !cg.Name.Contains("Farm"));
    
    public static IEnumerable<CategoryGroup> FilterToFarmCategories(
        this IEnumerable<CategoryGroup> categoryGroups)
        => categoryGroups.Where(cg => cg.Name.Contains("Farm"));
}