using Microsoft.Extensions.DependencyInjection;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.ViewModels.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<TransactionMemoOccurrenceViewModelBuilder>()
            .AddSingleton<TransactionYearAverageViewModelBuilder>()
            .AddSingleton<AmountViewModelBuilder>()
            .AddSingleton<TransactionMonthFlaggedViewModelBuilder>()
            .AddSingleton<CategoryYearAverageViewModelBuilder>();
}