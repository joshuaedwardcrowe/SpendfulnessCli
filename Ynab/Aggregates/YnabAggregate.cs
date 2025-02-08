namespace Ynab.Aggregates;

public class YnabAggregation<TAggregation> where TAggregation : class
{
    public IEnumerable<TAggregation> Aggregation { get; set; }
}