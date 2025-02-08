using Microsoft.Extensions.DependencyInjection;

namespace YnabProgressConsole.Compilation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<RecurringTransactionsViewModelCompiler>();

}