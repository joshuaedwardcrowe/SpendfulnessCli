using Microsoft.Extensions.DependencyInjection;
using Ynab.Collections;
using YnabProgressConsole.Compilation.RecurringTransactions;

namespace YnabProgressConsole.Compilation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedSingleton<IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName>,
                RecurringTransactionsViewModelBuilder>(typeof(TransactionsByMemoOccurrenceByPayeeName));

}