using System.Globalization;
using CsvHelper;
using Spendfulness.Database;

namespace SpendfulnessCli.Sync.Synchronisers;

public class YnabCalibrationSynchroniser : Synchroniser
{
    private readonly SpendfulnessBudgetClient _spendfulnessBudgetClient;

    public YnabCalibrationSynchroniser(SpendfulnessBudgetClient spendfulnessBudgetClient)
    {
        _spendfulnessBudgetClient = spendfulnessBudgetClient;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var csvRows = await GetCsvRows();
        
        await WriteCsvFile(csvRows);
    }

    private async Task<IEnumerable<YnabCalibrationCsvRow>> GetCsvRows()
    {
        // get budget
        var budget = await _spendfulnessBudgetClient.GetDefaultBudget();
    
        // get transactions
        var transactions = await budget.GetTransactions();

        var transactionsByCategoryName = transactions.GroupBy(t => t.CategoryName);
        
        var csvRows = new List<YnabCalibrationCsvRow>();
        
        foreach (var categoryGroup in transactionsByCategoryName)
        {
            var categoryName = categoryGroup.Key;

            var perYear = categoryGroup
                .GroupBy(t => t.Occured.Year)
                .Average(o => o.Sum(t => t.Amount));
            
            var perMonth = categoryGroup
                .GroupBy(t => t.Occured.Month)
                .Average(o => o.Sum(t => t.Amount));
            
            var csvRow = new YnabCalibrationCsvRow
            {
                CategoryName = categoryName,
                PerMonth = perMonth,
                PerYear = perYear
            };
            
            csvRows.Add(csvRow);
        }
        
        return csvRows;
    }

    private async Task WriteCsvFile(IEnumerable<YnabCalibrationCsvRow> csvRows)
    {
        var profileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var ynabCalibrationPath = $"{profileDirectoryPath}//ynab_calibration.csv";
    
        await using var writer = new StreamWriter(ynabCalibrationPath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(csvRows);
        
        Console.WriteLine($"CSV File has been saved to {ynabCalibrationPath}");
    }
}

public class YnabCalibrationCsvRow
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal PerMonth { get; set; }
    public decimal PerYear { get; set; }
}