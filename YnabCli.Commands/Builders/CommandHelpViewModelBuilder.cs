using YnabCli.Commands.Aggregate;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Builders;

public class CommandHelpViewModelBuilder : ViewModelBuilder<IEnumerable<CommandHelpAggregate>>
{
    protected override List<string> BuildColumnNames(IEnumerable<CommandHelpAggregate> evaluation)
        => [nameof(CommandHelpAggregate.Call), nameof(CommandHelpAggregate.Type), nameof(CommandHelpAggregate.Summary)];

    protected override List<List<object>> BuildRows(IEnumerable<CommandHelpAggregate> aggregates)
    {
        var rows = aggregates
            .Select(aggregate => new List<object>
            {
                aggregate.Call,
                aggregate.Type.ToString(),
                aggregate.Summary,
            })
            .ToList();

        return rows;
    }
}