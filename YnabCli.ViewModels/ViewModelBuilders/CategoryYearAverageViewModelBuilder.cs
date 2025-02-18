using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.Formatters;

namespace YnabCli.ViewModels.ViewModelBuilders;

public class CategoryYearAverageViewModelBuilder : 
    ViewModelBuilder<CategoryYearAverageAggregator, IEnumerable<CategoryYearAverageAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<CategoryYearAverageAggregate> evaluation)
    {
        var aggregatesIndexed = evaluation.ToList();
        
        var highestYearsCount = aggregatesIndexed.Max(y => y.AverageAmountByYears.Count);
        var highestYearsAggregate = aggregatesIndexed
            .First(y => y.AverageAmountByYears.Count == highestYearsCount);

        ColumnNames.Add("Category Name");
        ColumnNames.AddRange(highestYearsAggregate.AverageAmountByYears.Keys);

        return ColumnNames;
    }

    protected override List<List<object>> BuildRows(IEnumerable<CategoryYearAverageAggregate> aggregates)
    {
        var rows = new List<List<object>>();

        foreach (var aggregate in aggregates)
        {
            var currentRow = BuildIndividualRow(aggregate);
            rows.AddRange(currentRow.ToList());
        }

        return rows;
    }

    private IEnumerable<object> BuildIndividualRow(CategoryYearAverageAggregate aggregate)
    {
        yield return aggregate.CategoryName;
        
        // Skip 1 for the default 'Category Name' column
        for (var i = 1; i < ColumnNames.Count; i++)
        {
            var columnName = ColumnNames[i];

            if (!aggregate.AverageAmountByYears.TryGetValue(columnName, out var amount))
            {
                yield return string.Empty;
                continue;
            }

            yield return CurrencyDisplayFormatter.Format(amount);
        }
    }
}