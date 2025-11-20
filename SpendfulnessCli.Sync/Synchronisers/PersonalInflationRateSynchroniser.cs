using System.Globalization;
using CsvHelper;
using Spendfulness.Database;
using SpendfulnessCli.Abstractions.Calculators;

namespace SpendfulnessCli.Sync.Synchronisers;

public class PersonalInflationRateSynchroniser : Synchroniser
{
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;

    public PersonalInflationRateSynchroniser(SpendfulnessBudgetClient spendfulnessBudgetClient)
    {
        _spendfulnessBudgetClient = spendfulnessBudgetClient;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var csvRows = await GetCsvRows();
        
        await WriteCsvFile(csvRows);
    }
    
    private async Task<IEnumerable<PirCsvRow>> GetCsvRows()
    {
        // get budget
        var budget = await _spendfulnessBudgetClient.GetDefaultBudget();

        // get transactions
        var transactions = await budget.GetTransactions();

        var categoryThenYearThenTransactions = transactions
            .GroupBy(transaction => transaction.CategoryName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .GroupBy(tansaction => tansaction.Occured.Year)
                    .ToDictionary(tansaction => tansaction.Key, tansaction => tansaction));
        
        var csvRows = new List<PirCsvRow>();

        foreach (var group in categoryThenYearThenTransactions)
        {
            var categoryName = group.Key;
            var yearThenTransactions = group.Value;
            
            var yearlyExpenditures = new List<PirYearlyExpenditureCsvColumn>();

            for (var i = 0; i < yearThenTransactions.Count; i++)
            {
                decimal lastYearExpenditure = 0;

                if (i > 0)
                {
                    lastYearExpenditure = yearThenTransactions
                        .ElementAt(i - 1)
                        .Value
                        .Sum(t => t.Amount);
                }
                
                var thisYearExpenditure = yearThenTransactions.ElementAt(i).Value.Sum(t => t.Amount);
                
                var percDiff = PercentageCalculator.CalculateChange(lastYearExpenditure, thisYearExpenditure);
                
                var x = new PirYearlyExpenditureCsvColumn
                {
                    Year = yearThenTransactions.ElementAt(i).Key,
                    Expenditure = thisYearExpenditure,
                    Percentage =percDiff
                };
                
                yearlyExpenditures.Add(x);
            }
            
            var csvRow = new PirCsvRow
            {
                CategoryName = categoryName,
                YearlyExpenditures = yearlyExpenditures
            };
            
            csvRows.Add(csvRow);
        }

        return csvRows;
    }
    
    private async Task WriteCsvFile(IEnumerable<PirCsvRow> csvRows)
    {
        var profileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var ynabCalibrationPath = $"{profileDirectoryPath}//personal_inflation_rate.csv";

        await using var writer = new StreamWriter(ynabCalibrationPath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        
        // write constant header row
        csv.WriteField("Category");
        csv.WriteField(string.Empty);
        
        // write dynamic year headers
        var years = csvRows
            .SelectMany(x => x.YearlyExpenditures
                .Select(x => x.Year))
            .Distinct();

        foreach (var year in years)
        {
            csv.WriteField($"{year} Expenditure");
            csv.WriteField($"{year} % Difference");
            csv.WriteField(string.Empty);
        }
        
        await csv.NextRecordAsync();
        
        // write all the data rows
        foreach (var csvRow in csvRows)
        {
            csv.WriteField(csvRow.CategoryName);
            csv.WriteField(string.Empty);
            
            foreach (var yearlyExpenditure in csvRow.YearlyExpenditures)
            {
                csv.WriteField(yearlyExpenditure.Expenditure);
                csv.WriteField(yearlyExpenditure.Percentage);
                csv.WriteField(string.Empty);
            }
            
            await csv.NextRecordAsync();
        }
        
        
        Console.WriteLine($"CSV File has been saved to {ynabCalibrationPath}");
    }
}



public record PirCsvRow
{
    public string CategoryName { get; set; }
    
    public List<PirYearlyExpenditureCsvColumn> YearlyExpenditures { get; set; }
    
}

public record PirYearlyExpenditureCsvColumn
{
    public int Year { get; set; }
    public decimal Expenditure { get; set; }
    public decimal Percentage { get; set; }
}
