using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandGenerator : ICommandGenerator
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        return new RecurringTransactionsCommand();
    }
}