namespace YnabProgressConsole.Commands.RecurringTransactions;

public class RecurringTransactionsCommand : ICommand
{
    public const string CommandName = "recurring-transactions";
    public const string ShorthandCommandName = "rt";
    
    public static class ArgumentNames
    {
        public const string From = "from";
        public const string To = "to";
        public const string PayeeName = "payee-name";
        public const string MinimumOccurrences = "minimum-occurrences";
    }

    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public string? PayeeName { get; set; }
    public int? MinimumOccurrences { get; set; }
}