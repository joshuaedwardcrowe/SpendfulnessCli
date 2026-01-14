using System.Globalization;
using CsvHelper;
using Spendfulness.Aggregation.Aggregates;
using Spendfulness.Database;
using Spendfulness.Tools;
using Spendfulness.Tools.Percentages;
using SpendfulnessCli.Aggregation.Aggregator;
using Ynab;

namespace Spendfulness.CsvExports.PersonalInflationRate;

public class PersonalInflationRateCsvWriter(SpendfulnessBudgetClient spendfulnessBudgetClient) 
    : ICsvWriter<TransactionByYearsByCategoryGroupAggregate>
{
    public async Task Write(YnabListAggregator<TransactionByYearsByCategoryGroupAggregate> aggregator)
    {
        var profileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var ynabCalibrationPath = $"{profileDirectoryPath}//personal_inflation_rate.csv";

        await using var writer = new StreamWriter(ynabCalibrationPath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        var defaultBudget = await spendfulnessBudgetClient.GetDefaultBudget();
        
        var years = defaultBudget.GetYears();

        var aggregates = aggregator.Aggregate();
        
        // Write CSV Header
        WriteHeader(csv, years.Measurable);

        await csv.NextRecordAsync();

        // Write CSV Body
        await WriteBody(csv, years, aggregates);
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

    private async Task WriteBody(CsvWriter csv, BudgetYears years, IEnumerable<TransactionByYearsByCategoryGroupAggregate> aggregates)
    {
        foreach (var aggregate in aggregates)
        {
            // Write Category Group row with values
            // TODO: Category Formatter
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
    }
}