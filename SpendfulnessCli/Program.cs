// See https://aka.ms/new-console-template for more information

using KitCli;
using KitCli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Organisation;
using SpendfulnessCli.Commands.Personalisation;
using Spendfulness.Database;
using Spendfulness.Database.Cosmos;
using Spendfulness.Database.Sqlite;
using Spendfulness.OpenAI;
using SpendfulnessCli;
using SpendfulnessCli.Abstractions.Taxis;
using SpendfulnessCli.Aggregation.Extensions;
using SpendfulnessCli.Commands;
using SpendfulnessCli.Commands.Chat;
using SpendfulnessCli.Commands.Export.Csv;
using SpendfulnessCli.Commands.Reporting;
using SpendfulnessCli.Commands.Reusable;
using YnabSharp.Extensions;

var cliAppBuilder = new CliAppBuilder()
    .WithCli<SpendfulnessCliApp>();

// Add settings
cliAppBuilder
    .WithUserSecretSettings<SpendfulnessCliApp>()
    .WithJsonSettings("appsettings.json")
    .WithSettings<InstructionSettings>()
    .WithSettings<OpenAiSettings>()
    .WithSettings<CosmosSettings>();
    
// Add YNAB services
cliAppBuilder
    .WithServices(services => 
        services
            .AddYnab() // Speak to the YNAB API
            .AddYnabTransactionFactory<TaxiTransactionFactory>());


// Spendfulness db specific set up.
cliAppBuilder
    .WithConfiguredServices<CosmosSettings>((settings, services) =>
        services
            .AddSpendfulnessCosmos(settings));

// Spendfulness specific set up.
cliAppBuilder
    .WithServices(services =>
        services
            .AddSpendfulnessSqliteDb() 
            .AddSpendfulnessAggregatorCommandArtefacts() 
            .AddSpendfulnessCommands() 
            .AddSpendfulnessReportingCommands() 
            .AddSpendfulnessOrganisationCommands() 
            .AddSpendfulnessPersonalisationCommands() 
            .AddSpendfulnessReusableCommands()
            .AddSpendfulnessExportCsvCommands()
            .AddSpendfulnessChatCommands()); 

await cliAppBuilder.Run();