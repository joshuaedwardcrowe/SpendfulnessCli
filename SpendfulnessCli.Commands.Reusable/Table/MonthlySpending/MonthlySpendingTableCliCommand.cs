using Cli.Abstractions.Aggregators;
using Spendfulness.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

public record MonthlySpendingTableCliCommand(CliListAggregator<TransactionMonthTotalAggregate> Aggregator)
    : TableCliCommand;