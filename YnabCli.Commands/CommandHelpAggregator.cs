using YnabCli.ViewModels.Aggregator;

namespace YnabCli.Commands;

public abstract class CommandHelpAggregator : Aggregator<List<CommandHelpAggregate>>
{
    public override List<CommandHelpAggregate> Aggregate() => AggregateForCommand();
    
    protected abstract List<CommandHelpAggregate> AggregateForCommand();
}