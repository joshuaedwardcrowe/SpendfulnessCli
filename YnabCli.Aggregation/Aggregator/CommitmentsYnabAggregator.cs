using YnabCli.Database.Commitments;

namespace YnabCli.Aggregation.Aggregator;

public class CommitmentsYnabAggregator(ICollection<Commitment> commitments)
    : YnabAggregator<IEnumerable<Commitment>>(commitments)
{
    protected override IEnumerable<Commitment> GenerateAggregate() => Commitments;
}