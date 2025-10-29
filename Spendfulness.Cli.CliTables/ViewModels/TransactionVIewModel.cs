using Cli.Abstractions;

namespace Spendfulness.Cli.CliTables.ViewModels;

public abstract class TransactionVIewModel : CliTable
{
    private const string TransactionIdColumnName = "Transaction ID";
    private const string OccurredColumnName = "Occurred";
    private const string PayeeNameColumn = "Payee";
    private const string CategoryNameColumn = "Category";
    private const string AmountColumnName = "Amount";
    
    public static List<string> GetColumnNames() 
        => [
            TransactionIdColumnName,
            OccurredColumnName,
            PayeeNameColumn,
            CategoryNameColumn,
            AmountColumnName
        ];
}