using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.AverageYearlyPay;

public class AverageYearlyPayCommandHandler(ConfiguredBudgetClient budgetClient)
    : CommandHandler, ICommandHandler<AverageYearlyPayCommand>
{
    public async Task<ConsoleTable> Handle(AverageYearlyPayCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator();

        var viewModel = new TransactionYearAverageViewModelBuilder()
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return Compile(viewModel);
    }

    private async Task<ListAggregator<TransactionYearAverageAggregate>> PrepareAggregator()
    {
        var budget =  await budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionAveragePerYearAggregator(transactions);

        aggregator
            .BeforeAggregation(t => t.FilterToInflow())
            .BeforeAggregation(t => t.FilterToPayeeNames("BrightHR"))
            .BeforeAggregation(t => t.OrderByYear()); 

        return aggregator;
    }
}
