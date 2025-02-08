using Microsoft.Extensions.DependencyInjection;
using Ynab.Collections;
using YnabProgressConsole.Compilation.RecurringTransactions;
using YnabProgressConsole.Compilation.SalaryIncreases;

namespace YnabProgressConsole.Compilation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedSingleton<IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName>,
                TransactionsByMemoOccurrenceByPayeeNameViewModelBuilder>(
                typeof(TransactionsByMemoOccurrenceByPayeeName))
            .AddKeyedSingleton<IViewModelBuilder<AmountByYear>,
                AmountByYearViewModelBuilder>(typeof(AmountByYear));

}