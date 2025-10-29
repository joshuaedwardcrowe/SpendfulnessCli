using Cli.Spendfulness.Commands;
using Cli.Spendfulness.Commands.Aggregate;
using Spendfulness.Cli.Aggregation.Aggregator.ListAggregators;

namespace Cli.Ynab.Commands.Reporting.SpareMoney.Help;

public class SpareMoneyCommandHelpYnabAggregator : ListYnabAggregator<CommandHelpAggregate>
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