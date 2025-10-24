using Cli.Spendfulness.CliTables.ViewModels;
using Ynab;

namespace Cli.Spendfulness.CliTables.ViewModelBuilders;

public class TransactionsCliTableBuilder : CliTableBuilder<IEnumerable<Transaction>>
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
            transaction.Id,
            transaction.Occured,
            transaction.PayeeName,
            transaction.CategoryName,
            transaction.Amount
        ];
}