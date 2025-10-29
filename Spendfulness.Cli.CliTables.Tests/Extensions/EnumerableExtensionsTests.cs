using Cli.Abstractions;
using Spendfulness.Cli.CliTables.Extensions;

namespace Spendfulness.Cli.CliTables.Tests.Extensions;

[TestFixture]
public class EnumerableExtensionsTests
{
    [TestCase(CliTableSortOrder.Ascending, 1, 2)]
    [TestCase(CliTableSortOrder.Descending, 2, 1)]
    public void GivenEnumerablAndSortOrder_WhenSortBySortOrder_SortsBySortOrder(
        CliTableSortOrder sortOrder, int expectedFirstValue, int expectedSecondValue)
    {
        var collection = new List<int> { 1, 2 };

        var ordered = collection
            .OrderBySortOrder(sortOrder, value => value)
            .ToList();
        
        var firstValue = ordered.First();
        Assert.That(firstValue, Is.EqualTo(expectedFirstValue));
        
        var secondValue = ordered.Last();
        Assert.That(secondValue, Is.EqualTo(expectedSecondValue));
    }
}