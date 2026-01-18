using Spendfulness.Aggregation.Aggregates;
using SpendfulnessCli.Aggregation.Aggregator;
using Ynab;

namespace Spendfulness.Aggregation.Aggregator.ListAggregators;

public class CategoryYnabListAggregator(IEnumerable<CategoryGroup> categoryGroups) 
    : YnabListAggregator<CategoryAggregate>(categoryGroups)
{
    protected override IEnumerable<CategoryAggregate> GenerateAggregate()
        => CategoryGroups
            .SelectMany(categoryGroup => categoryGroup.Categories)
            .Select(category => new CategoryAggregate(category.Id, category.Name));
}