using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Aggregate;

namespace YnabCli.Commands.Reporting.SpareMoney.Help;

public class SpareMoneyCommandHelpAggregator : ListAggregator<CommandHelpAggregate>
{
    protected override IEnumerable<CommandHelpAggregate> ListAggregate()
        => [
            new(
                "/spare-money",
                CommandActionType.Command,
                "Get spare money after critical expenses"),

            new(
                "help",
                CommandActionType.SubCommand,
                "Get a list of all calls"),

            new(
                "--minus-savings",
                CommandActionType.Argument,
                "Get spare moeny after critical expenses, minus savings")
        ];
}