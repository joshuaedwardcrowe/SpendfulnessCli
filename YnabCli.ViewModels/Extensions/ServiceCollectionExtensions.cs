using Microsoft.Extensions.DependencyInjection;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.ViewModels.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<TransactionMemoOccurrenceViewModelBuilder>()
            .AddSingleton<TransactionYearAverageViewModelBuilder>()
            .AddSingleton<AmountViewModelBuilder>()
            .AddSingleton<TransactionMonthFlaggedViewModelBuilder>()
            .AddSingleton<CategoryYearAverageViewModelBuilder>();
}