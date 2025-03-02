using ConsoleTables;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.YearlyPay;

public class YearlyPayCommandHandler : CommandHandler, ICommandHandler<YearlyPayCommand>
{
    private readonly DbBudgetClient _budgetClient;
    private readonly TransactionYearAverageViewModelBuilder _averageViewModelBuilder;

    public YearlyPayCommandHandler(DbBudgetClient budgetClient, TransactionYearAverageViewModelBuilder averageViewModelBuilder)
    {
        _budgetClient = budgetClient;
        _averageViewModelBuilder = averageViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(YearlyPayCommand request, CancellationToken cancellationToken)
    {
        var budget =  await _budgetClient.GetDefaultBudget();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionYearAverageAggregator(transactions);

        var viewModel = _averageViewModelBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return Compile(viewModel);
    }
}
