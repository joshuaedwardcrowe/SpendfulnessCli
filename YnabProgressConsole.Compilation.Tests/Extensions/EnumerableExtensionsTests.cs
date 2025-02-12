using YnabProgressConsole.Compilation.Extensions;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Compilation.Tests.Extensions;

[TestFixture]
public class EnumerableExtensionsTests
{
    [TestCase(ViewModelSortOrder.Ascending, 1, 2)]
    [TestCase(ViewModelSortOrder.Descending, 2, 1)]
    public void GivenEnumerablAndSortOrder_WhenSortBySortOrder_SortsBySortOrder(
        ViewModelSortOrder sortOrder, int expectedFirstValue, int expectedSecondValue)
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