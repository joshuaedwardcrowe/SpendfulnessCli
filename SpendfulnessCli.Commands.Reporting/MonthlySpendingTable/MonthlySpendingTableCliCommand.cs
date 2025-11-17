using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

namespace SpendfulnessCli.Commands.Reporting.MonthlySpendingTable;




// TODO: How do I stop this being used like a normal command?
public record MonthlySpendingTableCliCommand : ContinuousCliCommand<IEnumerable<TransactionMonthTotalAggregate>>
{
    public MonthlySpendingTableCliCommand(ListYnabAggregator<TransactionMonthTotalAggregate> aggregator)
        : base(aggregator)
    {
    }
}

public class MonthlySpendingTableCliCommandGenerator : ICliCommandGenerator<ContinuousCliCommand<IEnumerable<TransactionMonthTotalAggregate>>>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        throw new NotImplementedException();
    }
}

