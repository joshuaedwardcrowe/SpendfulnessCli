using Microsoft.Extensions.DependencyInjection;
using Ynab.Aggregates;
using Ynab.Collections;
using YnabProgressConsole.Compilation.AmountByYear;
using YnabProgressConsole.Compilation.SpareMoney;
using YnabProgressConsole.Compilation.TransactionsByMemoOccurrenceByPayeeNameV;

namespace YnabProgressConsole.Compilation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleCompilation(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddKeyedSingleton<IGroupViewModelBuilder<TransactionsByMemoOccurrenceByPayeeName>,
                TransactionsByMemoOccurrenceByPayeeNameGroupViewModelBuilder>(
                typeof(TransactionsByMemoOccurrenceByPayeeName))
            .AddKeyedSingleton<IGroupViewModelBuilder<Ynab.Collections.AmountByYear>,
                AmountByYearGroupViewModelBuilder>(typeof(Ynab.Collections.AmountByYear))
            .AddKeyedSingleton<IAggregateViewModelBuilder<SpareMoneyAggregation, AccountBalanceAggregate>,
                SpareMoneyViewModelBuilder>(typeof(SpareMoneyAggregation));
}