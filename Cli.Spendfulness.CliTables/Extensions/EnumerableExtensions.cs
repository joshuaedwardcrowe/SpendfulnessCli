using Cli.Abstractions;

namespace Cli.Spendfulness.CliTables.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource> OrderBySortOrder<TSource, TKey>(
        this IEnumerable<TSource> source, CliTableSortOrder sortOrder, Func<TSource, TKey> sortFunc)
        => sortOrder == CliTableSortOrder.Ascending
            ? source.OrderBy(sortFunc)
            : source.OrderByDescending(sortFunc);
}