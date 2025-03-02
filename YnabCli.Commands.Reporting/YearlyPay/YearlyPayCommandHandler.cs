using ConsoleTables;
using YnabCli.Commands.Factories;
using YnabCli.Commands.Handlers;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Reporting.YearlyPay;

public class YearlyPayCommandHandler : CommandHandler, ICommandHandler<YearlyPayCommand>
{
    private readonly CommandBudgetGetter _budgetGetter;
    private readonly TransactionYearAverageViewModelBuilder _averageViewModelBuilder;

    public YearlyPayCommandHandler(CommandBudgetGetter budgetGetter, TransactionYearAverageViewModelBuilder averageViewModelBuilder)
    {
        _budgetGetter = budgetGetter;
        _averageViewModelBuilder = averageViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(YearlyPayCommand request, CancellationToken cancellationToken)
    {
        var budget =  await _budgetGetter.Get();
        
        var transactions = await budget.GetTransactions();
        
        var aggregator = new TransactionYearAverageAggregator(transactions);

        var viewModel = _averageViewModelBuilder
            .WithAggregator(aggregator)
            .WithRowCount(false)
            .Build();

        return Compile(viewModel);
    }
}
