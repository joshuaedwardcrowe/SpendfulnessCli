using Cli.Abstractions;

namespace Cli.Spendfulness.CliTables.ViewModels;

public class TransactionMonthFlaggedViewModel : CliTable
{
    public const string MonthColumnName = "Month";

    public static List<string> GetColumnNames() => [MonthColumnName];
}