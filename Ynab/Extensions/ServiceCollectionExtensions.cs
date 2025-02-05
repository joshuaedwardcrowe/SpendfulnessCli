using Microsoft.Extensions.DependencyInjection;
using Ynab.Clients;

namespace Ynab.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYnab(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddSingleton<YnabHttpClientFactory>();
        serviceCollection.AddSingleton<BudgetsClient>();
        return serviceCollection;
    }
}