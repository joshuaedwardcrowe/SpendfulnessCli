using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.MonthlySpending;

// TODO: Write unit tests.
public class MonthlySpendingCliCommandGenerator : ICliCommandGenerator<MonthlySpendingCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
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