using System.Reflection;
using Cli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SpendfulnessCli.Commands.Exports.YnabCalibration;

namespace SpendfulnessCli.Commands.Exports;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExportsCommands(this IServiceCollection serviceCollection)
    {
        var organisationCommandsAssembly = Assembly.GetAssembly(typeof(YnabCalibrationCliCommand));
        return serviceCollection.AddCommandsFromAssembly(organisationCommandsAssembly);
    }
}