using Cli.Spendfulness.Aggregation.Aggregates;
using Cli.Spendfulness.CliTables.Formatters;

namespace Cli.Spendfulness.CliTables.ViewModelBuilders;

public class CategoryYearAverageCliTableBuilder : CliTableBuilder<IEnumerable<CategoryYearAverageAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<CategoryYearAverageAggregate> evaluation)
    {
        var aggregatesIndexed = evaluation.ToList();
        
        var highestYearsCount = aggregatesIndexed.Max(y => y.AverageAmountByYears.Count);
        var highestYearsAggregate = aggregatesIndexed
            .First(y => y.AverageAmountByYears.Count == highestYearsCount);

        return new List<string> { "Category Name" }
            .Concat(highestYearsAggregate.AverageAmountByYears.Keys)
            .ToList();
    }

    protected override List<List<object>> BuildRows(IEnumerable<CategoryYearAverageAggregate> aggregates)
    {
        var aggregatesIndexed = aggregates.ToList();
        var columnNames = BuildColumnNames(aggregatesIndexed);
        
        var rows = new List<List<object>>();

        foreach (var aggregate in aggregatesIndexed)
        {
            var currentRow = BuildIndividualRow(columnNames, aggregate);
            rows.AddRange(currentRow.ToList());
        }

        return rows;
    }

    private IEnumerable<object> BuildIndividualRow(List<string> columnNames, CategoryYearAverageAggregate aggregate)
    {
        yield return aggregate.CategoryName;
        
        // Skip 1 for the default 'Category Name' column
        for (var i = 1; i < columnNames.Count; i++)
        {
            var columnName = columnNames[i];

            if (!aggregate.AverageAmountByYears.TryGetValue(columnName, out var amount))
            {
                yield return string.Empty;
                continue;
            }

            yield return CurrencyDisplayFormatter.Format(amount);
        }
    }
}