namespace Spendfulness.Cli.Aggregation.Aggregates;

public record TransactionMonthFlaggedAggregate(
    string Month,
    List<TransactionMonthFlaggedAmountAggregate> AmountAggregates);