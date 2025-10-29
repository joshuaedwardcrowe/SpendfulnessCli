using Spendfulness.Cli.Aggregation.Aggregates;
using Spendfulness.Cli.CliTables.ViewModels;

namespace Spendfulness.Cli.CliTables.ViewModelBuilders;

public class CategoryCliTableBuilder : CliTableBuilder<IEnumerable<CategoryAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<CategoryAggregate> evaluation)
        => CategoryViewModel.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<CategoryAggregate> aggregates)
        => aggregates
            .Select(a => new List<object> { a.CategoryName, a.CategoryId })
            .ToList();
}