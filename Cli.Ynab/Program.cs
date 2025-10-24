// See https://aka.ms/new-console-template for more information

using Cli;
using Cli.Commands.Abstractions;
using Cli.Instructions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Extensions;
using YnabCli.Commands.Extensions;
using YnabCli.Commands.Organisation;
using YnabCli.Commands.Personalisation;
using YnabCli.Commands.Reporting;
using YnabCli.Database;

var serviceProvider = new ServiceCollection()
    .AddCli<YnabCliApp>()
    .AddYnab() // Speak to the YNAB API
    .AddDb() // Store data in an SQLite databaase.
    .AddCommands() // Convert them into MediatR requests
    .AddReportingCommands() // Commands that work with YNAB data
    .AddOrganisationCommands() // Commands that help organise the data
    .AddPersonalisationCommands() // Commands for CRUD with db data
    .AddConsoleInstructions() // Understand terminal commands
    .BuildServiceProvider();

var cliApp = serviceProvider.GetRequiredService<CliApp>();

await cliApp.Run();