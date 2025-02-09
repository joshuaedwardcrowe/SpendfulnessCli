using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandGenerator : ICommandGenerator, ITypedCommandGenerator<RecurringTransactionsCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        var payeeNameArgument = arguments.OfType<string>(RecurringTransactionsCommand.ArgumentNames.PayeeName);
        var minimumOccurrencesArgument = arguments.OfType<int>(RecurringTransactionsCommand.ArgumentNames.MinimumOccurrences);

        return new RecurringTransactionsCommand
        {
            PayeeName = payeeNameArgument?.ArgumentValue,
            MinimumOccurrences = minimumOccurrencesArgument?.ArgumentValue
        };
    }
}