using Cli.Commands.Abstractions;

namespace SpendfulnessCli.Commands.Reporting.RecurringTransactions;

public record RecurringTransactionsCliCommand : CliCommand
{
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