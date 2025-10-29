// See https://aka.ms/new-console-template for more information

using Cli;
using Cli.Instructions.Extensions;
using SpendfulnessCli.Commands.Extensions;
using SpendfulnessCli.Commands.Organisation;
using SpendfulnessCli.Commands.Personalisation;
using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Database;
using SpendfulnessCli;
using SpendfulnessCli.Abstractions.Taxis;
using SpendfulnessCli.Commands.Reporting;
using Ynab.Extensions;

// TODO: CLI - I wonder if I could simplify this to new CliBuilder() or something.
var serviceProvider = new ServiceCollection()
    .AddCli<SpendfulnessCliApp>()
    .AddYnab() // Speak to the YNAB API
    .AddYnabTransactionFactory<TaxiTransactionFactory>()
    .AddSpendfulnessDb() // Store data in an SQLite databaase.
    .AddCommands() // Convert them into MediatR requests
    .AddReportingCommands() // Commands that work with YNAB data
    .AddOrganisationCommands() // Commands that help organise the data
    .AddPersonalisationCommands() // Commands for CRUD with db data
    .AddCliInstructions() // Understand terminal commands
    .BuildServiceProvider();

var cliApp = serviceProvider.GetRequiredService<CliApp>();

await cliApp.Run();