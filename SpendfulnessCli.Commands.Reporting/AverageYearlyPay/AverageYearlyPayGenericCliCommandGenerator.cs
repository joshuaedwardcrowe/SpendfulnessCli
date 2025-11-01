using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.AverageYearlyPay;

public class AverageYearlyPayGenericCliCommandGenerator : ICliCommandGenerator<AverageYearlyPayCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        return new AverageYearlyPayCliCommand();
    }
}