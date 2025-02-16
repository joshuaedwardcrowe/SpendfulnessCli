using Microsoft.Extensions.DependencyInjection;
using YnabProgressConsole.Compilation.ViewModelBuilders;

namespace YnabProgressConsole.Compilation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<TransactionMemoOccurrenceViewModelBuilder>()
            .AddSingleton<TransactionYearAverageViewModelBuilder>()
            .AddSingleton<CompanyDeductedBalanceViewModelBuilder>()
            .AddSingleton<TransactionMonthFlaggedViewModelBuilder>();
}