using Cli.Spendfulness.Database.Commitments;

namespace Cli.Spendfulness.Aggregation.Aggregator;

public class CommitmentsYnabAggregator(ICollection<Commitment> commitments)
    : YnabAggregator<IEnumerable<Commitment>>(commitments)
{
    protected override IEnumerable<Commitment> GenerateAggregate() => Commitments;
}