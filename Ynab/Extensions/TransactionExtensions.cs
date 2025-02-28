using Microsoft.VisualBasic;
using Ynab.Collections;
using Ynab.Connected;
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

    public static IEnumerable<Transaction> FilterFrom(
        this IEnumerable<Transaction> transactions, DateOnly startDate)
    {
        var dateFrom = startDate.ToDateTime(TimeOnly.MinValue);
        return transactions.Where(t => t.Occured >= dateFrom);
    }

    public static IEnumerable<Transaction> FilterTo(
        this IEnumerable<Transaction> transactions, DateOnly stopDate)
    {
        var dateFrom = stopDate.ToDateTime(TimeOnly.MinValue);
        return transactions.Where(t => t.Occured <= dateFrom);
    }

    public static IEnumerable<Transaction> FilterToSpending(this IEnumerable<Transaction> transactions)
        => transactions.Where(transaction =>
            !transaction.IsTransfer && 
            !YnabConstants.AutomatedPayeeNames.Contains(transaction.PayeeName));
    
    public static IEnumerable<Transaction> OrderByYear(
        this IEnumerable<Transaction> transactions)
        => transactions.OrderBy(transaction => transaction.Occured.Year);
    
    public static IEnumerable<TransactionsByYear> GroupByYear(this IEnumerable<Transaction> transactions)
    {
        var groups = transactions.GroupBy(transaction =>
            IdentifierSanitiser.SanitiseForYear(transaction.Occured));

        foreach (var group in groups)
        {
            yield return new TransactionsByYear(group.Key, group.ToList());
        }
    }
    
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

    public static IEnumerable<TransactionsByCategory> GroupByCategory(this IEnumerable<Transaction> transactions)
        => transactions
            .Where(transaction => !YnabConstants.AutomatedCategoryNames.Contains(transaction.CategoryName))
            .GroupBy(transaction => transaction.CategoryName)
            .Select(group => new TransactionsByCategory(group.Key, group.ToList()));
}