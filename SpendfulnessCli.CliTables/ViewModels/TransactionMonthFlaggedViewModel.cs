using Cli.Abstractions;
using Cli.Abstractions.Tables;

namespace SpendfulnessCli.CliTables.ViewModels;

public class TransactionMonthFlaggedViewModel : CliTable
{
    public const string MonthColumnName = "Month";

    public static List<string> GetColumnNames() => [MonthColumnName];
}