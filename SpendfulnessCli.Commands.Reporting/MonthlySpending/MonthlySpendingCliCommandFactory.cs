using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Artefacts;
using Cli.Commands.Abstractions.Factories;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.MonthlySpending;

// TODO: Write unit tests.
public class MonthlySpendingCliCommandFactory : ICliCommandFactory<MonthlySpendingCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> properties)
    {
        var categoryIdArgument = instruction
            .Arguments
            .OfType<Guid>(MonthlySpendingCliCommand.ArgumentNames.CategoryId);

        return new MonthlySpendingCliCommand
        {
            CategoryId = categoryIdArgument?.ArgumentValue
        };
    }
}