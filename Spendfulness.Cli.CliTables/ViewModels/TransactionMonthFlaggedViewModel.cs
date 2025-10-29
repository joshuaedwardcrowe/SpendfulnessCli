using Cli.Abstractions;

namespace Spendfulness.Cli.CliTables.ViewModels;

public class TransactionMonthFlaggedViewModel : CliTable
{
    public const string MonthColumnName = "Month";

    public static List<string> GetColumnNames() => [MonthColumnName];
}