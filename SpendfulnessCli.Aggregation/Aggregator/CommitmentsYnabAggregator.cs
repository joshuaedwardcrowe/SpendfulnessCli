using Spendfulness.Database.Commitments;

namespace SpendfulnessCli.Aggregation.Aggregator;

public class CommitmentsYnabAggregator(ICollection<Commitment> commitments)
    : YnabAggregator<IEnumerable<Commitment>>(commitments)
{
    protected override IEnumerable<Commitment> GenerateAggregate() => Commitments;
}