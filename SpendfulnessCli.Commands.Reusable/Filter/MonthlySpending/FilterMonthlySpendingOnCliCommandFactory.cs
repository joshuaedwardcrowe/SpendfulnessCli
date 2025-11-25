using Cli.Commands.Abstractions.Artefacts;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;
using SpendfulnessCli.Commands.Reusable.MonthlySpending;

namespace SpendfulnessCli.Commands.Reusable.Filter.MonthlySpending;

public abstract class FilterMonthlySpendingOnCliCommandFactory
    : MonthlySpendingCliCommandFactory
{
    public override bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var previouslyCalledMonthlySpendingCommand = base.CanCreateWhen(instruction, artefacts);

        var filterOnArgument = instruction
            .Arguments
            .OfType<string>(FilterCliCommand.ArgumentNames.FilterOn);
        
        return previouslyCalledMonthlySpendingCommand && filterOnArgument != null;
    }
}