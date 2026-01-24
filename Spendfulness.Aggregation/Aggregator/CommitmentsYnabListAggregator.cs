using Spendfulness.Database.Sqlite.Commitments;

namespace Spendfulness.Aggregation.Aggregator;

public class CommitmentsYnabListAggregator(ICollection<Commitment> commitments) : YnabListAggregator<Commitment>(commitments)
{
    protected override IEnumerable<Commitment> GenerateAggregate() => Commitments;
}