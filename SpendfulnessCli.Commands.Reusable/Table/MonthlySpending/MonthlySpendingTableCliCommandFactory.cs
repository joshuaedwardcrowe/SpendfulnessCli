using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Attributes;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Final;
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Aggregation.Aggregates;
using SpendfulnessCli.Commands.Reporting.MonthlySpending;

namespace SpendfulnessCli.Commands.Reusable.Table.MonthlySpending;

[FactoryFor(typeof(TableCliCommand))]
public class MonthlySpendingTableCliCommandFactory : ICliCommandFactory<MonthlySpendingTableCliCommand>
{
    public bool CanGenerate(CliInstruction instruction, List<CliCommandProperty> properties)
        => properties.AnyCommandRan<MonthlySpendingCliCommand>();
    
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
    {
        var monthlySpendingAggregator = properties.GetListAggregator<TransactionMonthTotalAggregate>();
        if (monthlySpendingAggregator is null)
        {
            return new PrerequisitesNotMetCliCommand(
                nameof(CliCommandListAggregatorOutcome<TransactionMonthTotalAggregate>));
        }
        
        return new MonthlySpendingTableCliCommand(monthlySpendingAggregator);
    }
}

public record PrerequisitesNotMetCliCommand(params string[] PrerequisitesNotMet) : CliCommand;

public class RequirementsNotMetCliCommandHandler : ICliCommandHandler<PrerequisitesNotMetCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(PrerequisitesNotMetCliCommand command, CancellationToken cancellationToken)
    {
        var outcome = new CliCommandPrerequisiteNotMetOutcome(command.PrerequisitesNotMet);

        return Task.FromResult<CliCommandOutcome[]>([outcome]);
    }
}