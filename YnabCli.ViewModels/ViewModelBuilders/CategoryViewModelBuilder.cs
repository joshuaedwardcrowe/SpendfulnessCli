using YnabCli.Aggregation.Aggregates;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.ViewModels.ViewModelBuilders;

public class CategoryViewModelBuilder : ViewModelBuilder<IEnumerable<CategoryAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<CategoryAggregate> evaluation)
        => CategoryViewModel.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<CategoryAggregate> aggregates)
        => aggregates
            .Select(a => new List<object> { a.CategoryName, a.CategoryId })
            .ToList();
}