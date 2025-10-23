using Cli.Instructions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Extensions;
using YnabCli;
using YnabCli.Commands.Extensions;
using YnabCli.Commands.Organisation;
using YnabCli.Commands.Personalisation;
using YnabCli.Commands.Reporting;
using YnabCli.Database;

// TODO: Reorganise for CliApp. In theory you can do anything as long as you use ICommand framework.
var serviceProvider = new ServiceCollection()
    .AddYnab() // Speak to the YNAB API
    .AddCommands() // Convert them into MediatR requests
    .AddReportingCommands() // Commands that work with YNAB data
    .AddOrganisationCommands() // Commands that help organise the data
    .AddPersonalisationCommands() // Commands for CRUD with db data
    .AddInstructions() // Understand terminal commands
    .AddDb() // Store da
    .AddSingleton<YnabCliApp>() // Front-end
    .BuildServiceProvider();

var app = serviceProvider.GetRequiredService<YnabCliApp>();

 await app.Run();

