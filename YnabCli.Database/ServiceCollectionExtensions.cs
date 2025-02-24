using Microsoft.Extensions.DependencyInjection;

namespace YnabCli.Database;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddYnabCliDb(this IServiceCollection services)
        => services.AddDbContext<YnabCliDbContext>();
}