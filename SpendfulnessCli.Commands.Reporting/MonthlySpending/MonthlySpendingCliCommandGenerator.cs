using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Generators;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.MonthlySpending;

// TODO: Write unit tests.
public class MonthlySpendingCliCommandGenerator : ICliCommandGenerator<MonthlySpendingCliCommand>
{
    public CliCommand Generate(CliInstruction instruction, List<CliCommandProperty> properties)
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