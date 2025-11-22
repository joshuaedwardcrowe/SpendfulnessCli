using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Factories;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace SpendfulnessCli.Commands.Reporting.FlagChanges;

public class FlagChangesCliCommandFactory : ICliCommandFactory<FlagChangesCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandProperty> properties)
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