using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Aggregation.Aggregates;
using YnabCli.Aggregation.Aggregator.ListAggregators;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.AverageYearlyPay;

public class AverageYearlyPayCommandHandler : CommandHandler, ICommandHandler<AverageYearlyPayCommand>
{
    private readonly ConfiguredBudgetClient _budgetClient;
    private readonly TransactionYearAverageViewModelBuilder _averageViewModelBuilder;

    public AverageYearlyPayCommandHandler(ConfiguredBudgetClient budgetClient, TransactionYearAverageViewModelBuilder averageViewModelBuilder)
    {
        _budgetClient = budgetClient;
        _averageViewModelBuilder = averageViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(AverageYearlyPayCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);

        var viewModel = _averageViewModelBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return Compile(viewModel);
    }

    private async Task<ListAggregator<TransactionYearAverageAggregate>> PrepareAggregator(AverageYearlyPayCommand command)
    {
        var budget =  await _budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionAveragePerYearAggregator(transactions);

        aggregator
            .BeforeAggregation(t => t.FilterToInflow())
            .BeforeAggregation(t => t.FilterToPayeeNames("BrightHR"))
            .BeforeAggregation(t => t.OrderByYear()); 

        return aggregator;
    }
}
