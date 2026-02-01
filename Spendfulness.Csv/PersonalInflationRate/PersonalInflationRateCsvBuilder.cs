using Spendfulness.Aggregation.Aggregates;
using Spendfulness.Aggregation.Aggregator;
using Spendfulness.Tools;
using Spendfulness.Tools.Percentages;
using YnabSharp;

namespace Spendfulness.Csv.PersonalInflationRate;

public class PersonalInflationRateCsvBuilder : ICsvBuilder<TransactionByYearsByCategoryGroupAggregate>
{
    private Budget? _budget;
    private YnabListAggregator<TransactionByYearsByCategoryGroupAggregate>? _aggregator;
    
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
        var years = _budget!.GetYears();

        var aggregates = _aggregator!.Aggregate();

        var headerCsvRow = BuildHeaderCsvRow(years);

        var csvRows = new List<CsvRow>
        {
            headerCsvRow
        };

        foreach (var aggregate in aggregates)
        {
            var categoryGroupCsvRow = BuildCategoryGroupCsvRow(aggregate, years);
            csvRows.Add(categoryGroupCsvRow);
            
            foreach (var categoryAggregate in aggregate.CategoryAggregates)
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
            
            var currentYearSpend = aggregate.TotalAmountForYear(measurableYear);
            var priorYearSpend = aggregate.TotalAmountForYear(priorYear);

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

                var currentYearSpend = aggregate.TotalAmountForYear(measurableYear);
                var priorYearSpend = aggregate.TotalAmountForYear(priorYear);

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

            var currentYearSpend = aggregate.TotalAmountForYear(measurableYear);
            var priorYearSpend = aggregate.TotalAmountForYear(priorYear);

            var totalSpendText = CurrencyDisplayFormatter.Format(currentYearSpend);
            csvColumns.Add(totalSpendText);

            var percentChange = PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
            var percentageChangeText = PercentageDisplayFormatter.Format(percentChange);
            csvColumns.Add(percentageChangeText);
        }
        
        var averagePercentageChange = aggregate
            .TransactionsByYears
            .Where(tby => years.Measurable.Contains(tby.Year))
            .Average(tby =>
            {
                var measurableYear = tby.Year;
                var priorYear = measurableYear - 1;
                
                var currentYearSpend = aggregate.TotalAmountForYear(measurableYear);
                var priorYearSpend = aggregate.TotalAmountForYear(priorYear);

                return PercentageCalculator.CalculateChangeDecimal(priorYearSpend, currentYearSpend);
            });
        
        var averagePercentageChangeText = PercentageDisplayFormatter.Format(averagePercentageChange);
        csvColumns.Add(averagePercentageChangeText);

        return new CsvRow(csvColumns);
    }
}