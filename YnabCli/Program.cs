using Microsoft.Extensions.DependencyInjection;
using Ynab.Extensions;
using YnabCli;
using YnabCli.Commands;
using YnabCli.Commands.Extensions;
using YnabCli.Commands.Personalisation;
using YnabCli.Commands.Reporting;
using YnabCli.Database;
using YnabCli.Instructions;
using YnabCli.Instructions.Extensions;
using YnabCli.ViewModels.Extensions;

var serviceProvider = new ServiceCollection()
    .AddYnab() // Speak to the YNAB API
    .AddCompilation() // Compile into ConsoleTables.
    .AddCommands() // Convert them into MediatR requests
    .AddReportingCommands() // Commands that work with YNAB data
    .AddPersonalisationCommands() // Commands that work with db data
    .AddInstructions() // Understand terminal commands
    .AddDb() // Store data
    .AddSingleton<ConsoleApplication>() // Front-end
    .BuildServiceProvider();

var app = serviceProvider.GetRequiredService<ConsoleApplication>();

 await app.Run();

