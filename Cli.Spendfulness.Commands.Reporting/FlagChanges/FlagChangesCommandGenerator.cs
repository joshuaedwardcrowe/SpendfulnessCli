using Cli.Commands.Abstractions;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Ynab.Commands.Reporting.FlagChanges;

public class FlagChangesCommandGenerator : ICommandGenerator<FlagChangesCommand>
{
    public ICommand Generate(string? subCommandName, List<ConsoleInstructionArgument> arguments)
    {
        // TODO: I dont like that this isnt more generic!
        var from = arguments.OfType<DateOnly>(FlagChangesCommand.ArgumentNames.From);
        var to = arguments.OfType<DateOnly>(FlagChangesCommand.ArgumentNames.To);

        return new FlagChangesCommand
        {
            From = from?.ArgumentValue,
            To = to?.ArgumentValue
        };
    }
}