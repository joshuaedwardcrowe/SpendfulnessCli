using Spendfulness.Database.Commitments;

namespace SpendfulnessCli.Aggregation.Aggregator;

public class CommitmentsYnabListAggregator(ICollection<Commitment> commitments) : YnabListAggregator<Commitment>(commitments)
{
    protected override IEnumerable<Commitment> GenerateAggregate() => Commitments;
}