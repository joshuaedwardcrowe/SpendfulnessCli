namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommand : ICommand
{
    public const string CommandName = "recurring-transactions";
    public const string ShorthandCommandName = "rt";
    
    public static class ArgumentNames
    {
        public const string PayeeName = "payee-name";
        public const string MinimumOccurrences = "minimum-occurrences";
    }

    public string? PayeeName { get; set; }
    public int? MinimumOccurrences { get; set; }
}