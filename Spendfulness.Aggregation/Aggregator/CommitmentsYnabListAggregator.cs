using Spendfulness.Database.Commitments;
using SpendfulnessCli.Aggregation.Aggregator;

namespace Spendfulness.Aggregation.Aggregator;

public class CommitmentsYnabListAggregator(ICollection<Commitment> commitments) : YnabListAggregator<Commitment>(commitments)
{
    protected override IEnumerable<Commitment> GenerateAggregate() => Commitments;
}