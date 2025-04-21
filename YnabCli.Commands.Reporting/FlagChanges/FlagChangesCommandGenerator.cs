using YnabCli.Commands.Generators;
using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.FlagChanges;

public class FlagChangesCommandGenerator : ICommandGenerator<FlagChangesCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
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