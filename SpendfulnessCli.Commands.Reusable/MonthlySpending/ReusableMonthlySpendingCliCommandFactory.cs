using Cli.Commands.Abstractions.Artefacts;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Reporting.MonthlySpending;
using SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

namespace SpendfulnessCli.Commands.Reusable.MonthlySpending;

public abstract class ReusableMonthlySpendingCliCommandFactory
{
    public virtual bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var ranAggregatorCommand = artefacts.LastCommandRanWas<MonthlySpendingCliCommand>();
        var ranFilterCommand = artefacts.LastCommandRanWas<FilterMonthlySpendingCliCommand>();

        return ranAggregatorCommand || ranFilterCommand;
    }
}