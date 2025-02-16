using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.FlagChanges;

public class FlagChangesCommandGenerator : ICommandGenerator, ITypedCommandGenerator<FlagChangesCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
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