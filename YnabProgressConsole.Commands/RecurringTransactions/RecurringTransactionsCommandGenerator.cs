using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandGenerator : ICommandGenerator, ITypedCommandGenerator<RecurringTransactionsCommand>
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        var payeeName = RecurringTransactionsCommand.ArgumentNames.PayeeName;
        
        var payeeNameArgument = arguments
            .Where(arg => arg.ArgumentName == payeeName)
            .OfType<TypedInstructionArgument<string>>()
            .FirstOrDefault();
        
        var minimumOccurrences = RecurringTransactionsCommand.ArgumentNames.MinimumOccurrences;
        var minimumOccurrencesArgument = arguments.
            Where(argument => argument.ArgumentName ==  minimumOccurrences)
            .OfType<TypedInstructionArgument<int>>()
            .FirstOrDefault();

        return new RecurringTransactionsCommand
        {
            PayeeName = payeeNameArgument?.ArgumentValue,
            MinimumOccurrences = minimumOccurrencesArgument?.ArgumentValue
        };
    }
}