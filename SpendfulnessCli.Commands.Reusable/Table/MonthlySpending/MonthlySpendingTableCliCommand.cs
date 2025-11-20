using Cli.Abstractions;
using SpendfulnessCli.Aggregation.Aggregates;

namespace SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

public record MonthlySpendingTableCliCommand(CliListAggregator<TransactionMonthTotalAggregate> Aggregator)
    : TableCliCommand;