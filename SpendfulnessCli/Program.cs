// See https://aka.ms/new-console-template for more information

using Cli;
using Cli.Instructions.Abstractions;
using SpendfulnessCli.Commands.Organisation;
using SpendfulnessCli.Commands.Personalisation;
using Spendfulness.Database;
using SpendfulnessCli;
using SpendfulnessCli.Abstractions.Taxis;
using SpendfulnessCli.Aggregation.Extensions;
using SpendfulnessCli.Commands;
using SpendfulnessCli.Commands.Export.Csv;
using SpendfulnessCli.Commands.Reporting;
using SpendfulnessCli.Commands.Reusable;
using Ynab.Extensions;

var cliAppBuilder = new CliAppBuilder()
    .WithCli<SpendfulnessCliApp>();

// Add settings
cliAppBuilder
    .WithUserSecretSettings<SpendfulnessCliApp>()
    .WithJsonSettings("appsettings.json")
    .WithSettings<InstructionSettings>();
    
// Add YNAB services
cliAppBuilder
    .WithCustomServices(services => 
        services
            .AddYnab() // Speak to the YNAB API
            .AddYnabTransactionFactory<TaxiTransactionFactory>());


// Spendfulness specific set up.
cliAppBuilder
    .WithCustomServices(services =>
        services
            .AddSpendfulnessDb() 
            .AddSpendfulnessAggregatorCommandArtefacts() 
            .AddSpendfulnessCommands() 
            .AddSpendfulnessReportingCommands() 
            .AddSpendfulnessOrganisationCommands() 
            .AddSpendfulnessPersonalisationCommands() 
            .AddSpendfulnessReusableCommands()
            .AddSpendfulnessExportCsvCommands()); 

await cliAppBuilder.Run();