using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.FlagChanges;

public class FlagChangesCliCommandGenerator : ICliCommandGenerator<FlagChangesCliCommand>
{
    public CliCommand Generate(CliInstruction instruction)
    {
        // TODO: I dont like that this isnt more generic!
        var from = instruction
            .Arguments
            .OfType<DateOnly>(FlagChangesCliCommand.ArgumentNames.From);
        
        var to = instruction.
            Arguments
            .OfType<DateOnly>(FlagChangesCliCommand.ArgumentNames.To);

        return new FlagChangesCliCommand
        {
            From = from?.ArgumentValue,
            To = to?.ArgumentValue
        };
    }
}