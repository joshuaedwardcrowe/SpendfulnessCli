using Spendfulness.Aggregation.Aggregates;
using YnabSharp;
using YnabSharp.Collections;
using YnabSharp.Extensions;

namespace Spendfulness.Aggregation.Aggregator.ListAggregators;

public class TransactionByYearsByCategoryGroupAggregator(
    BudgetYears budgetYears,
    IEnumerable<Transaction> transactions,
    IEnumerable<CategoryGroup> categoryGroups)
    : YnabListAggregator<TransactionByYearsByCategoryGroupAggregate>(transactions, categoryGroups)
{
    protected override IEnumerable<TransactionByYearsByCategoryGroupAggregate> GenerateAggregate()
    {
        var splitTransactions = Transactions
            .Where(transaction => transaction.SplitTransactions.Any())
            .SelectMany(transaction => transaction.SplitTransactions);
        
        var mergedTransactions = Transactions
            .Where(transaction => !transaction.SplitTransactions.Any())
            .Concat(splitTransactions);
        
        var transactionsByCategoryByYears = mergedTransactions
            .GroupByCategory()
            .GroupByYear()
            .ToList();

        var spendingCategoryGroups = CategoryGroups
            .FilterToSpendingCategories();
        
        var categoryGroupAggregates = new List<TransactionByYearsByCategoryGroupAggregate>();
        
        foreach (var categoryGroup in spendingCategoryGroups)
        {
            var categoryAggregates = new List<TransactionByYearsByCategoryAggregate>();

            foreach (var category in categoryGroup.Categories)
            {
                var byYears = new List<SplitTransactionsByYear>();
                
                var categoryTransactions = transactionsByCategoryByYears
                    .FirstOrDefault(tcy => tcy.CategoryId == category.Id);
                
                foreach (var year in budgetYears.All)
                {
                    var transactionByYear = categoryTransactions
                        ?.TransactionsByYear
                        .FirstOrDefault(tby => tby.Year == year);
                    
                    var transactionsInYear = transactionByYear ?? new SplitTransactionsByYear(year, new List<SplitTransactions>());
                    
                    byYears.Add(transactionsInYear);
                }
                
                var categoryAggregate = new TransactionByYearsByCategoryAggregate(category.Name, byYears);
                
                categoryAggregates.Add(categoryAggregate);
            }
            
            var categoryGroupAggregate = new TransactionByYearsByCategoryGroupAggregate(categoryGroup.Name, categoryAggregates);

            categoryGroupAggregates.Add(categoryGroupAggregate);
        }

        return categoryGroupAggregates;
    }
}