using ConsoleTables;
using Ynab.Extensions;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.Aggregates;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;
using YnabCli.ViewModels.ViewModels;

namespace YnabCli.Commands.Reporting.YearlyPay;

public class YearlyPayCommandHandler : CommandHandler, ICommandHandler<YearlyPayCommand>
{
    private readonly ConfiguredBudgetClient _budgetClient;
    private readonly TransactionYearAverageViewModelBuilder _averageViewModelBuilder;

    public YearlyPayCommandHandler(ConfiguredBudgetClient budgetClient, TransactionYearAverageViewModelBuilder averageViewModelBuilder)
    {
        _budgetClient = budgetClient;
        _averageViewModelBuilder = averageViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(YearlyPayCommand command, CancellationToken cancellationToken)
    {
        var aggregator = await PrepareAggregator(command);

        var viewModel = _averageViewModelBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return Compile(viewModel);
    }

    private async Task<ListAggregator<TransactionYearAverageAggregate>> PrepareAggregator(YearlyPayCommand command)
    {
        var budget =  await _budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionYearAverageAggregator(transactions);

        aggregator
            .BeforeAggregation(t => t.FilterToInflow())
            .BeforeAggregation(t => t.FilterByPayeeName("BrightHR"))
            .BeforeAggregation(t => t.OrderByYear()); 

        return aggregator;
    }
}
