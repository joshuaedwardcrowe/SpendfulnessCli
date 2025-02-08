using Ynab.Collections;
using Ynab.Generators;

namespace Ynab.Extensions;

public static class FlaggedTransactionsForMonthExtensions
{
    public static IEnumerable<FlaggedTransactionsForMonth> GroupByFlags(
        this IEnumerable<TransactionsForMonth> transactionsForMonths)
    {
        foreach (var transactionsForMonth in transactionsForMonths)
        {
            var transactionsForFlags = transactionsForMonth
                .Transactions
                .Where(t => t.FlagName != null)
                .GroupBy(t => IdentifierSanitiser.SanitiseForFlag(t.FlagName, t.FlagColour))
                .Select(transactionGroup => new TransactionsForFlag
                {
                    Flag = transactionGroup.Key,
                    Transactions = transactionGroup.ToList()
                });

            yield return new FlaggedTransactionsForMonth
            {
                Month= transactionsForMonth.Month,
                TransactionsForFlags = transactionsForFlags.ToList()
            };
        }
    }
}