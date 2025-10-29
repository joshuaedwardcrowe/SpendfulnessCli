using System.Reflection;
using Cli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SpendfulnessCli.Commands.Reporting.SpareMoney;

namespace SpendfulnessCli.Commands.Reporting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddReportingCommands(this IServiceCollection serviceCollection)
    {
        var reportingCommandsAssembly = Assembly.GetAssembly(typeof(SpareMoneyCliCommand));
        return serviceCollection.AddCommandsFromAssembly(reportingCommandsAssembly);
    }
}