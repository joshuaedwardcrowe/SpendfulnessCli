using Microsoft.Extensions.DependencyInjection;
using Ynab.Collections;
using YnabProgressConsole.Compilation.AmountByYear;
using YnabProgressConsole.Compilation.TransactionsByMemoOccurrenceByPayeeNameV;

namespace YnabProgressConsole.Compilation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedSingleton<IViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName>,
                TransactionsByMemoOccurrenceByPayeeNameViewModelBuilder>(
                typeof(TransactionsByMemoOccurrenceByPayeeName))
            .AddKeyedSingleton<IViewModelBuilder<Ynab.Collections.AmountByYear>,
                AmountByYearViewModelBuilder>(typeof(Ynab.Collections.AmountByYear));

}