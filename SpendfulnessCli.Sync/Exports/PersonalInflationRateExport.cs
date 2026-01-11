using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Hosting;
using Spendfulness.Database;
using SpendfulnessCli.Aggregation.Calculators;
using SpendfulnessCli.CliTables.Formatters;
using Ynab;
using Ynab.Collections;
using Ynab.Connected;
using Ynab.Extensions;

namespace SpendfulnessCli.Sync.Exports;

public class PersonalInflationRateExport(SpendfulnessBudgetClient spendfulnessBudgetClient) : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var profileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var ynabCalibrationPath = $"{profileDirectoryPath}//personal_inflation_rate.csv";
        
        await using var writer = new StreamWriter(ynabCalibrationPath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        var defaultBudget = await spendfulnessBudgetClient.GetDefaultBudget();
        
        var years = GetYears(defaultBudget);
        
        var aggregates =  await GetAggregates(defaultBudget, years);
        
        // Write CSV Header
        WriteHeader(csv, years.Measurable);
        
        await csv.NextRecordAsync();

        // Write CSV Body
        foreach (var aggregate in aggregates)
        {
            // Write Category Group row with values
            csv.WriteField($"{aggregate.CategoryGroupName} (Group)");

            foreach (var measurableYear in years.Measurable)
            {
                csv.WriteField(string.Empty);

                var priorYear = measurableYear - 1;

                // Sum current year spend for all categories in the group
                var currentYearSpend = aggregate.Aggregates
                    .Select(cat => cat.TransactionsByYears
                        .First(tby => tby.Year == measurableYear)
                        .SplitTransactions.Sum(transaction => transaction.Amount))
                    .Sum();

                // Sum prior year spend for all categories in the group
                var priorYearSpend = aggregate.Aggregates
                    .Select(cat => cat.TransactionsByYears
                        .First(tby => tby.Year == priorYear)
                        .SplitTransactions
                        .Sum(transaction => transaction.Amount))
                    .Sum();

                var totalSpendText = CurrencyDisplayFormatter.Format(currentYearSpend);
                csv.WriteField(totalSpendText);

                var percentChange = PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
                var percentageChangeText = PercentageDisplayFormatter.Format(percentChange);
                csv.WriteField(percentageChangeText);
            }

            csv.WriteField(string.Empty);

            // Calculate average percent change for the group
            var groupAveragePercentChange = years.Measurable
                .Select(measurableYear => {
                    var priorYear = measurableYear - 1;
                    var currentYearSpend = aggregate.Aggregates
                        .Select(cat => cat.TransactionsByYears
                            .First(tby => tby.Year == measurableYear)
                            .SplitTransactions
                            .Sum(transaction => transaction.Amount))
                        .Sum();
                    var priorYearSpend = aggregate.Aggregates
                        .Select(cat => cat.TransactionsByYears
                            .First(tby => tby.Year == priorYear)
                            .SplitTransactions
                            .Sum(transaction => transaction.Amount))
                        .Sum();
                    return PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
                })
                .DefaultIfEmpty(0)
                .Average();

            var groupAveragePercentChangeText = PercentageDisplayFormatter.Format(groupAveragePercentChange);
            csv.WriteField(groupAveragePercentChangeText);

            await csv.NextRecordAsync();

            foreach (var thing in aggregate.Aggregates)
            {
                // Write row for category
                csv.WriteField(thing.CategoryName);
                
                foreach (var measurableYear in years.Measurable)
                {
                    csv.WriteField(string.Empty);
                    
                    var priorYear = measurableYear - 1;

                    var currentYearSpend = thing.TransactionsByYears
                        .First(tby => tby.Year == measurableYear)
                        .SplitTransactions
                        .Sum(transaction => transaction.Amount);

                    var priorYearSpend = thing.TransactionsByYears
                        .First(tby => tby.Year == priorYear)
                        .SplitTransactions
                        .Sum(transaction => transaction.Amount);

                    var totalSpendText = CurrencyDisplayFormatter.Format(currentYearSpend);
                    csv.WriteField(totalSpendText);

                    var percentChange = PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
                    
                    var percentageChangeText = PercentageDisplayFormatter.Format(percentChange);
                    
                    csv.WriteField(percentageChangeText);
                }

                csv.WriteField(string.Empty);

                var op = thing
                    .TransactionsByYears
                    .Where(tby => years.Measurable.Contains(tby.Year))
                    .Average(tby =>
                    {
                        var currentYearSpend = thing
                            .TransactionsByYears
                            .First(tby2 => tby2.Year == tby.Year)
                            .SplitTransactions
                            .Sum(transaction => transaction.Amount);

                        var priorYearSpend = thing.TransactionsByYears
                            .First(tby2 => tby2.Year == tby.Year - 1)
                            .SplitTransactions
                            .Sum(transaction => transaction.Amount);

                        return PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
                    });
                
                var averagePercentageChangeText = PercentageDisplayFormatter.Format(op);
                csv.WriteField(averagePercentageChangeText);
                
                await csv.NextRecordAsync();
            }
            
            await csv.NextRecordAsync();
            
            
        }
        
        Console.WriteLine("Should have been written");
    }

    public class SomeAggregateCollection
    {
        public string CategoryGroupName { get; set; }
        public IEnumerable<SomeAggregate> Aggregates { get; set; }
    }

    public class SomeAggregate
    {
        public string CategoryName { get; set; }
        public IEnumerable<SplitTransactionsByYear> TransactionsByYears { get; set; }
    }

    private async Task<List<SomeAggregateCollection>> GetAggregates(ConnectedBudget budget, Years years)
    {
        var transactions = await budget.GetTransactions();
        var categoryGroups = await budget.GetCategoryGroups();
        
        var transactionsList = transactions.ToList();

        // Aggregator might start here.
        var splitTransactions = transactionsList
            .Where(transaction => transaction.SplitTransactions.Any())
            .SelectMany(transaction => transaction.SplitTransactions);

        var mergedTransactions = transactionsList
            .Where(transaction => !transaction.SplitTransactions.Any())
            .Concat(splitTransactions);
            
        var transactionsByCategoryByYears = mergedTransactions
            .GroupByCategory()
            .GroupByYear()
            .ToList();

        var spendingCategoryGroups = categoryGroups
            .FilterToSpendingCategories();
        
        // --- Below is an aggregation process.---
        var someAggregateCollections = new List<SomeAggregateCollection>();

        // Every group should show.
        foreach (var categoryGroup in spendingCategoryGroups)
        {
            var someAggregates = new List<SomeAggregate>();

            foreach (var category in categoryGroup.Categories)
            {
                var byYears = new List<SplitTransactionsByYear>();
                
                var categoryTransactions = transactionsByCategoryByYears
                    .FirstOrDefault(tcy => tcy.CategoryId == category.Id);
                
                foreach (var year in years.All)
                {
                    var transactionByYear = categoryTransactions?.TransactionsByYear.FirstOrDefault(tby => tby.Year == year);
                    
                    var transactionsInYear = transactionByYear ?? new SplitTransactionsByYear(year, new List<SplitTransactions>());
                    
                    byYears.Add(transactionsInYear);
                }
                
                var agg = new SomeAggregate
                {
                    CategoryName = category.Name,
                    TransactionsByYears = byYears
                };
                
                someAggregates.Add(agg);
            }
            
            var collection = new SomeAggregateCollection
            {
                CategoryGroupName = categoryGroup.Name,
                Aggregates = someAggregates
            };
            
            someAggregateCollections.Add(collection);
        }

        return someAggregateCollections;
    }

    public class Years
    {
        public List<int> All { get; set; }
        public List<int> Measurable { get; set; }
    }
    
    private Years GetYears(Budget budget)
    {
        var budgetActiveYearCount = budget.LastActive.Year - budget.Created.Year; // e.g. 3

        var budgetActiveYears = Enumerable
            .Range(budget.Created.Year, budgetActiveYearCount)
            .ToList();

        var measurableYears = budgetActiveYears
                
            // Need to miss off first year as there's no prior year to compare with.
            .Skip(1)

            // Need to miss off last (current year) as it's incomplete.
            .Take(budgetActiveYearCount - 1)
            
            .ToList();

        return new Years
        {
            All = budgetActiveYears,
            Measurable = measurableYears
        };
    }
    
    private void WriteHeader(CsvWriter csv, List<int> measurableYears)
    {
        csv.WriteField("Category");

        foreach (var measurableYear in measurableYears)
        {
            csv.WriteField(string.Empty);
            
            var totalSpendText = $"{measurableYear} Total Spend";
            csv.WriteField(totalSpendText);
            
            var versusText = $"{measurableYear} vs {measurableYear - 1} % Change";
            csv.WriteField(versusText);
        }
        
        csv.WriteField(string.Empty);
        csv.WriteField("Average % Change");
    }
}












