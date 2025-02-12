using Ynab.Collections;
using Ynab.Sanitisers;

namespace Ynab.Extensions;

public static class TransactionExtensions
{
    public static IEnumerable<Transaction> FilterByCategories(
        this IEnumerable<Transaction> transactions, IEnumerable<Guid> categoryIds) 
            => transactions.Where(transaction => transaction.CategoryId.HasValue && 
                                                 categoryIds.Contains(transaction.CategoryId.Value));
    
    public static IEnumerable<Transaction> FilterToInflow(
        this IEnumerable<Transaction> transactions) 
            => transactions.Where(transaction => transaction.Amount > 0);
    
    public static IEnumerable<Transaction> FilterByPayeeName(
        this IEnumerable<Transaction> transactions, params string[] payeeNames)
            => transactions.Where(t => payeeNames.Contains(t.PayeeName));

    public static IEnumerable<Transaction> FilterToSpending(this IEnumerable<Transaction> transactions)
        => transactions.Where(transaction =>
            !transaction.IsTransfer && 
            !YnabConstants.AutomatedPayeeNames.Contains(transaction.PayeeName));

    public static IEnumerable<TransactionsByMonth> GroupByMonth(
        this IEnumerable<Transaction> transactions)
    {
        var groups = transactions.GroupBy(transaction =>
            IdentifierSanitiser.SanitiseForMonth(transaction.Occured));

        foreach (var group in groups)
        {
            yield return new TransactionsByMonth
            {
                Month = group.Key,
                Transactions = group
            };
        }
    }
    
    public static IEnumerable<TransactionsByPayeeName> GroupByPayeeName(
        this IEnumerable<Transaction> transactions, string? payeeName = null)
    {
        var groups = transactions
            .GroupBy(transaction => transaction.PayeeName)
            .OrderByDescending(group => group.Count());

        foreach (var group in groups)
        {
            yield return new TransactionsByPayeeName
            {
                PayeeName = group.Key,
                Transactions = group.ToList()
            };
        }
    }

    public static IEnumerable<AmountByYear> AverageByYear(
        this IEnumerable<Transaction> transactions)
    {
        var groups = transactions
            .OrderBy(transaction => transaction.Occured.Year)
            .GroupBy(transaction => IdentifierSanitiser.SanitiseForYear(transaction.Occured));

        foreach (var group in groups)
        {
            var average = group.Average(transaction => transaction.Amount);
            var sanitised = DecimalPlaceSanitiser.Sanitise(average);
            
            yield return new AmountByYear
            {
                Year = group.Key,
                AverageAmount = sanitised
            };
        }
    }
}