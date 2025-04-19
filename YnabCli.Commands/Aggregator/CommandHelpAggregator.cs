using YnabCli.Aggregation.Aggregator;
using YnabCli.Commands.Aggregate;

namespace YnabCli.Commands.Aggregator;

public abstract class CommandHelpAggregator : Aggregator<List<CommandHelpAggregate>>
{
    public override List<CommandHelpAggregate> Aggregate() => AggregateForCommand();
    
    protected abstract List<CommandHelpAggregate> AggregateForCommand();
}