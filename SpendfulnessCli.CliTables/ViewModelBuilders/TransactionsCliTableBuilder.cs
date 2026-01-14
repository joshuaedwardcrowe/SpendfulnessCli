using Spendfulness.Formatters;
using SpendfulnessCli.CliTables.ViewModels;
using Ynab;

namespace SpendfulnessCli.CliTables.ViewModelBuilders;

public class TransactionsCliTableBuilder : CliTableBuilder<Transaction>
{
    protected override List<string> BuildColumnNames(IEnumerable<Transaction> evaluation)
        => TransactionVIewModel.GetColumnNames();

    protected override List<List<object>> BuildRows(IEnumerable<Transaction> aggregates)
        => aggregates
            .Select(BuildIndividualRow)
            .Select(row => row.ToList())
            .ToList();
    
    private IEnumerable<object> BuildIndividualRow(Transaction transaction) 
        => [
            transaction.Occured.ToShortDateString(),
            transaction.PayeeName,
            transaction.CategoryName,
            CurrencyDisplayFormatter.Format(transaction.Amount)
        ];
}