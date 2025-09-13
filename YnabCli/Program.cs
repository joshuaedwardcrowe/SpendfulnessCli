using Microsoft.Extensions.DependencyInjection;
using Ynab.Extensions;
using YnabCli;
using YnabCli.Commands.Extensions;
using YnabCli.Commands.Organisation;
using YnabCli.Commands.Personalisation;
using YnabCli.Commands.Reporting;
using YnabCli.Database;
using YnabCli.Instructions.Extensions;

var serviceProvider = new ServiceCollection()
    .AddYnab() // Speak to the YNAB API
    .AddCommands() // Convert them into MediatR requests
    .AddReportingCommands() // Commands that work with YNAB data
    .AddOrganisationCommands() // Commands that help organise the data
    .AddPersonalisationCommands() // Commands for CRUD with db data
    .AddInstructions() // Understand terminal commands
    .AddDb() // Store data
    .AddSingleton<ConsoleApplication>() // Front-end
    .BuildServiceProvider();

var app = serviceProvider.GetRequiredService<ConsoleApplication>();

 await app.Run();

