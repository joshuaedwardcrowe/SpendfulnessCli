using Microsoft.Extensions.Hosting;
using Spendfulness.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Csv;
using Spendfulness.Csv.PersonalInflationRate;
using Spendfulness.Database;

namespace SpendfulnessCli.Sync.Exports;

public class PersonalInflationRateExport : BackgroundService
{
    private readonly SpendfulnessBudgetClient spendfulnessBudgetClient;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var defaultBudget = await spendfulnessBudgetClient.GetDefaultBudget();
        
        var transactions = await defaultBudget.GetTransactions();
        var categoryGroups = await defaultBudget.GetCategoryGroups();
        
        var years = defaultBudget.GetYears();

        var aggregator = new TransactionByYearsByCategoryGroupAggregator(years, transactions, categoryGroups);

        var csv = new PersonalInflationRateCsvBuilder()
            .WithBudget(defaultBudget)
            .WithAggregator(aggregator)
            .Build();

        var writer = new CsvWriter(csv);

        var profileDirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var ynabCalibrationPath = $"{profileDirectoryPath}//personal_inflation_rate.csv";
        
        await writer.Write(ynabCalibrationPath);

        // await WriteCsv(years, aggregates);
        
        Console.WriteLine("Should have been written");
    }
}












