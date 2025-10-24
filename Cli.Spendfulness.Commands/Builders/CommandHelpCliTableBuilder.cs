using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Cli.Spendfulness.Commands.Aggregate;

namespace Cli.Spendfulness.Commands.Builders;

public class CommandHelpCliTableBuilder : CliTableBuilder<IEnumerable<CommandHelpAggregate>>
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