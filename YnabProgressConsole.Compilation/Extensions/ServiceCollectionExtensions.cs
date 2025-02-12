using Microsoft.Extensions.DependencyInjection;
using Ynab.Collections;
using YnabProgressConsole.Compilation.Aggregates;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModelBuilders;

namespace YnabProgressConsole.Compilation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddSingleton<ITransactionMemoOccurrenceViewModelBuilder, TransactionMemoOccurrenceViewModelBuilder>()
            .AddKeyedSingleton<IGroupViewModelBuilder<AmountByYear>,
                AmountByYearGroupViewModelBuilder>(typeof(AmountByYear))
            .AddKeyedSingleton<IEvaluationViewModelBuilder<CategoryDeductedBalanceEvaluator, decimal>,
                CompanyDeductedBalanceEvaluator>(typeof(CategoryDeductedBalanceEvaluator));
}