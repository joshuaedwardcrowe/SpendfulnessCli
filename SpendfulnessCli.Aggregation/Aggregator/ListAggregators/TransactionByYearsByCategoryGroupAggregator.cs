using SpendfulnessCli.Aggregation.Aggregates;
using Ynab;
using Ynab.Collections;
using Ynab.Extensions;

namespace SpendfulnessCli.Aggregation.Aggregator.ListAggregators;

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
        
        // --- Below is an aggregation process.---
        var someAggregateCollections = new List<TransactionByYearsByCategoryGroupAggregate>();

        // Every group should show.
        foreach (var categoryGroup in spendingCategoryGroups)
        {
            var someAggregates = new List<TransactionByYearsByCategoryAggregate>();

            foreach (var category in categoryGroup.Categories)
            {
                var byYears = new List<SplitTransactionsByYear>();
                
                var categoryTransactions = transactionsByCategoryByYears
                    .FirstOrDefault(tcy => tcy.CategoryId == category.Id);
                
                foreach (var year in budgetYears.All)
                {
                    var transactionByYear = categoryTransactions?.TransactionsByYear.FirstOrDefault(tby => tby.Year == year);
                    
                    var transactionsInYear = transactionByYear ?? new SplitTransactionsByYear(year, new List<SplitTransactions>());
                    
                    byYears.Add(transactionsInYear);
                }
                
                var agg = new TransactionByYearsByCategoryAggregate
                {
                    CategoryName = category.Name,
                    TransactionsByYears = byYears
                };
                
                someAggregates.Add(agg);
            }
            
            var collection = new TransactionByYearsByCategoryGroupAggregate
            {
                CategoryGroupName = categoryGroup.Name,
                Aggregates = someAggregates
            };
            
            someAggregateCollections.Add(collection);
        }

        return someAggregateCollections;
    }
}