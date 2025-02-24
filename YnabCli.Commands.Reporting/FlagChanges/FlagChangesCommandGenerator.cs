using YnabCli.Instructions.Arguments;

namespace YnabCli.Commands.Reporting.FlagChanges;

public class FlagChangesCommandGenerator : ICommandGenerator, ITypedCommandGenerator<FlagChangesCommand>
{
    public ICommand Generate(string? subCommandName, List<InstructionArgument> arguments)
    {
        var from = arguments.OfType<DateOnly>(FlagChangesCommand.ArgumentNames.From);
        var to = arguments.OfType<DateOnly>(FlagChangesCommand.ArgumentNames.To);

        return new FlagChangesCommand
        {
            From = from?.ArgumentValue,
            To = to?.ArgumentValue
        };
    }
}