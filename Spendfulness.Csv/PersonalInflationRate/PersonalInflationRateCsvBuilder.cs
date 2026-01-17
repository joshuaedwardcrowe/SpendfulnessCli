using Spendfulness.Aggregation.Aggregates;
using Spendfulness.Tools;
using Spendfulness.Tools.Percentages;
using SpendfulnessCli.Aggregation.Aggregator;
using Ynab;

namespace Spendfulness.Csv.PersonalInflationRate;

public class PersonalInflationRateCsvBuilder : ICsvBuilder<TransactionByYearsByCategoryGroupAggregate>
{
    private Budget _budget;
    private YnabListAggregator<TransactionByYearsByCategoryGroupAggregate> _aggregator;
    
    public ICsvBuilder<TransactionByYearsByCategoryGroupAggregate> WithBudget(Budget budget)
    {
        _budget = budget;
        return this;
    }

    public ICsvBuilder<TransactionByYearsByCategoryGroupAggregate> WithAggregator(YnabListAggregator<TransactionByYearsByCategoryGroupAggregate> aggregator)
    {
        _aggregator = aggregator;
        return this;
    }

    public Csv Build()
    {
        var years = _budget.GetYears();

        var aggregates = _aggregator.Aggregate();

        var headerCsvRow = BuildHeaderCsvRow(years);

        var csvRows = new List<CsvRow>
        {
            headerCsvRow
        };

        foreach (var aggregate in aggregates)
        {
            var categoryGroupCsvRow = BuildCategoryGroupCsvRow(aggregate, years);
            csvRows.Add(categoryGroupCsvRow);
            
            foreach (var categoryAggregate in aggregate.Aggregates)
            {
                var categoryCsvRow = BuildCategoryCsvRow(categoryAggregate, years);
                csvRows.Add(categoryCsvRow);
            }
        }

        return new Csv(csvRows);
    }

    private CsvRow BuildHeaderCsvRow(BudgetYears years)
    {
        var csvAxisColumn = new List<string> { "Category" };

        var measurableYearColumns = years
            .Measurable
            .SelectMany(measurableYear => new List<string>
            {
                $"{measurableYear} Total Spend",
                $"{measurableYear} vs {measurableYear - 1} % Change"
            });

        var lastColumn = new List<string>
        {
            "Average % Change"
        };

        var csvColumns = csvAxisColumn
            .Concat(measurableYearColumns)
            .Concat(lastColumn)
            .ToList();

        return new CsvRow(csvColumns);
    }

    private CsvRow BuildCategoryGroupCsvRow(TransactionByYearsByCategoryGroupAggregate aggregate, BudgetYears years)
    {
        var csvColumns = new List<string> { aggregate.CategoryGroupName };

        foreach (var measurableYear in years.Measurable)
        {
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
            csvColumns.Add(totalSpendText);

            var percentChange = PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
            var percentageChangeText = PercentageDisplayFormatter.Format(percentChange);
            csvColumns.Add(percentageChangeText);
        }

        var groupAveragePercentChange = years.Measurable
            .Select(measurableYear =>
            {
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
        csvColumns.Add(groupAveragePercentChangeText);

        return new CsvRow(csvColumns);
    }

    private CsvRow BuildCategoryCsvRow(TransactionByYearsByCategoryAggregate aggregate, BudgetYears years)
    {
        var csvColumns = new List<string> { aggregate.CategoryName };
        
        foreach (var measurableYear in years.Measurable)
        {
            var priorYear = measurableYear - 1;

            var currentYearSpend = aggregate.TransactionsByYears
                .First(tby => tby.Year == measurableYear)
                .SplitTransactions
                .Sum(transaction => transaction.Amount);

            var priorYearSpend = aggregate.TransactionsByYears
                .First(tby => tby.Year == priorYear)
                .SplitTransactions
                .Sum(transaction => transaction.Amount);

            var totalSpendText = CurrencyDisplayFormatter.Format(currentYearSpend);
            csvColumns.Add(totalSpendText);

            var percentChange = PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
            var percentageChangeText = PercentageDisplayFormatter.Format(percentChange);
            csvColumns.Add(percentageChangeText);
        }
        
        var op = aggregate
            .TransactionsByYears
            .Where(tby => years.Measurable.Contains(tby.Year))
            .Average(tby =>
            {
                var currentYearSpend = aggregate
                    .TransactionsByYears
                    .First(tby2 => tby2.Year == tby.Year)
                    .SplitTransactions
                    .Sum(transaction => transaction.Amount);

                var priorYearSpend = aggregate.TransactionsByYears
                    .First(tby2 => tby2.Year == tby.Year - 1)
                    .SplitTransactions
                    .Sum(transaction => transaction.Amount);

                return PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
            });
        
        var averagePercentageChangeText = PercentageDisplayFormatter.Format(op);
        csvColumns.Add(averagePercentageChangeText);

        return new CsvRow(csvColumns);
    }
}