// See https://aka.ms/new-console-template for more information

using Cli;
using Cli.Instructions.Extensions;
using Cli.Spendfulness.Commands.Extensions;
using Cli.Spendfulness.Commands.Organisation;
using Cli.Spendfulness.Commands.Personalisation;
using Spendfulness.Cli;
using Spendfulness.Cli.Abstractions.Taxis;
using Cli.Ynab.Commands.Reporting;
using Microsoft.Extensions.DependencyInjection;
using Spendfulness.Database;
using Ynab.Extensions;

// TODO: CLI - I wonder if I could simplify this to new CliBuilder() or something.
var serviceProvider = new ServiceCollection()
    .AddCli<SpendfulnessCli>()
    .AddYnab() // Speak to the YNAB API
    .AddYnabTransactionFactory<TaxiTransactionFactory>()
    .AddSpendfulnessDb() // Store data in an SQLite databaase.
    .AddCommands() // Convert them into MediatR requests
    .AddReportingCommands() // Commands that work with YNAB data
    .AddOrganisationCommands() // Commands that help organise the data
    .AddPersonalisationCommands() // Commands for CRUD with db data
    .AddCliInstructions() // Understand terminal commands
    .BuildServiceProvider();

var cliApp = serviceProvider.GetRequiredService<OriginalCli>();

await cliApp.Run();