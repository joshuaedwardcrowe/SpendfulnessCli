using SpendfulnessCli.Aggregation.Aggregator;
using SpendfulnessCli.Commands.Aggregate;

namespace SpendfulnessCli.Commands.Reporting.SpareMoney.Help;

public class SpareMoneyCommandHelpYnabListAggregator : YnabListAggregator<CommandHelpAggregate>
{
    protected override IEnumerable<CommandHelpAggregate> GenerateAggregate()
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