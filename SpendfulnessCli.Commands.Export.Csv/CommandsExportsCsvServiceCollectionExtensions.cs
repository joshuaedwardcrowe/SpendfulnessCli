using System.Reflection;
using Cli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SpendfulnessCli.Commands.Export.Csv.PersonalInflationRate;

namespace SpendfulnessCli.Commands.Export.Csv;

public static class CommandsExportsCsvServiceCollectionExtensions
{
    public static IServiceCollection AddSpendfulnessExportCsvCommands(this IServiceCollection serviceCollection)
    {
        var organisationCommandsAssembly = Assembly.GetAssembly(typeof(PersonalInflationRateExportCsvCliCommand));
        return serviceCollection.AddCommandsFromAssembly(organisationCommandsAssembly);
    }
}