using Cli.Commands;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Extensions;
using Cli.Commands.Abstractions.Io;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Extensions;
using Cli.Workflow;
using Microsoft.Extensions.DependencyInjection;

namespace Cli;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCli<TCliApp>(this IServiceCollection serviceCollection) where TCliApp : OriginalCli
    {
        serviceCollection.AddCliInstructions();
        serviceCollection.AddCommandsFromAssembly(typeof(ExitCommand).Assembly);
        
        serviceCollection.AddSingleton<CliWorkflowCommandProvider>();
        serviceCollection.AddSingleton<CliWorkflow>();

        serviceCollection.AddSingleton<CliIo>();
        serviceCollection.AddSingleton<CliCommandOutcomeIo>();
        
        serviceCollection.AddSingleton<OriginalCli, TCliApp>();
        
        return serviceCollection;
    }
}