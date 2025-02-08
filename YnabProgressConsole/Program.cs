using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Extensions;
using YnabProgressConsole;
using YnabProgressConsole.Extensions;

var serviceProvider = new ServiceCollection()
    .AddYnab()
    .AddMediatR(cfg => 
        cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
    .AddCommandGenerators()
    .AddViewModelConstructors()
    .AddInstructionParsing()
    .AddSingleton<ConsoleApplication>()
    .BuildServiceProvider();

var app = serviceProvider.GetService<ConsoleApplication>();

 await app.Run();

