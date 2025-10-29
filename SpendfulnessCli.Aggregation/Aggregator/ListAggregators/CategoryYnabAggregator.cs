using SpendfulnessCli.Aggregation.Aggregates;
using Ynab;

namespace SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

public class CategoryYnabAggregator(IEnumerable<CategoryGroup> categoryGroups) 
    : ListYnabAggregator<CategoryAggregate>(categoryGroups)
{
    protected override IEnumerable<CategoryAggregate> ListAggregate()
        => CategoryGroups
            .SelectMany(categoryGroup => categoryGroup.Categories)
            .Select(category => new CategoryAggregate(category.Id, category.Name));
}