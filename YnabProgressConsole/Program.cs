using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole;
using YnabProgressConsole.Commands;
using YnabProgressConsole.Commands.CommandList;
using YnabProgressConsole.Commands.RecurringTransactions;
using YnabProgressConsole.Extensions;
using YnabProgressConsole.ViewModels;

var serviceProvider = new ServiceCollection()
    .AddYnab()
    .AddMediatR(cfg => 
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
    .AddCommandGenerators()
    .AddViewModelConstructors()
    .AddSingleton<ConsoleApplication>()
    .BuildServiceProvider();

var app = serviceProvider.GetService<ConsoleApplication>();

 await app.Run();

