using YnabProgressConsole.Instructions.InstructionArguments;

namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommandGenerator : ICommandGenerator
{
    public ICommand Generate(List<InstructionArgument> arguments)
    {
        var requiredArgumentName = RecurringTransactionsCommand.ArgumentNames.MinimumOccurrences;
        
        var minimumOccurrencesArgument = arguments.
            Where(argument => argument.ArgumentName ==  requiredArgumentName)
            .OfType<TypedInstructionArgument<int>>()
            .FirstOrDefault();

        return new RecurringTransactionsCommand
        {
            MinimumOccurrences = minimumOccurrencesArgument?.ArgumentValue
        };
    }
}