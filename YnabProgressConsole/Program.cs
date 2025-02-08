using Microsoft.Extensions.DependencyInjection;
using Ynab.Extensions;
using YnabProgressConsole;
using YnabProgressConsole.Commands.Extensions;
using YnabProgressConsole.Compilation.Extensions;
using YnabProgressConsole.Instructions.Extensions;

var serviceProvider = new ServiceCollection()
    .AddConsoleCompilation() // Compile into ConsoleTables.
    .AddYnab() // Speak to the YNAB API
    .AddConsoleCommands() // Convert them into MediatR requests
    .AddConsoleInstructions() // Understand terminal commands
    .AddSingleton<ConsoleApplication>() // Front-end
    .BuildServiceProvider();

var app = serviceProvider.GetService<ConsoleApplication>();

 await app.Run();

