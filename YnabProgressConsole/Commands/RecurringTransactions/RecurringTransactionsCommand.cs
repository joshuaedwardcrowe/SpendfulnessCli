namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommand : ICommand
{
    public const string CommandName = "recurring-transactions";
    public static class ArgumentNames
    {
        public const string MinimumOccurrences = "minimum-occurences";
    }

    public int? MinimumOccurrences { get; set; }
}