using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.FlagChanges;

public class FlagChangesCommandGenerator : ICommandGenerator, ITypedCommandGenerator<FlagChangesCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments) => new FlagChangesCommand();
}