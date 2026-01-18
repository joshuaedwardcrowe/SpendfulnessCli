using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Aggregation.Aggregator.ListAggregators;
using Spendfulness.Csv.PersonalInflationRate;
using Spendfulness.Database;
using Spendfulness.Tools;

namespace SpendfulnessCli.Commands.Export.Csv.PersonalInflationRate;

public class PersonalInflationRateExportCsvCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient)
    : CliCommandHandler, ICliCommandHandler<PersonalInflationRateExportCsvCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(PersonalInflationRateExportCsvCliCommand command, CancellationToken cancellationToken)
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
        
        var filePath = command.OutputFileSystemInfo.GetFilePath("personal-inflation-rate", FileFormat.Csv);
        
        await csv.Write(filePath);
        
        return OutcomeAs(
            "Personal inflation rate CSV export completed.",
            $"Please refer to file at: {filePath}");
    }
}

