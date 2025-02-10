using Microsoft.Extensions.DependencyInjection;
using Ynab.Collections;
using YnabProgressConsole.Compilation.AmountByYearView;
using YnabProgressConsole.Compilation.CompanyDeductedBalanceView;
using YnabProgressConsole.Compilation.SpareMoneyView;
using YnabProgressConsole.Compilation.TransactionsByMemoOccurrenceByPayeeNameView;

namespace YnabProgressConsole.Compilation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedSingleton<IGroupViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName>,
                TransactionsByMemoOccurrenceByPayeeNameGroupViewModelBuilder>(
                typeof(TransactionsByMemoOccurrenceByPayeeName))
            .AddKeyedSingleton<IGroupViewModelBuilder<AmountByYear>,
                AmountByYearGroupViewModelBuilder>(typeof(AmountByYear))
            .AddKeyedSingleton<IEvaluationViewModelBuilder<CategoryDeductedBalanceEvaluator, decimal>,
                CompanyDeductedBalanceEvaluator>(typeof(CategoryDeductedBalanceEvaluator));
}