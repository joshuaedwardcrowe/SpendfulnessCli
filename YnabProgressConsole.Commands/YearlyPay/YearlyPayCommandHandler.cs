using ConsoleTables;
using Ynab.Clients;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModelBuilders;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Commands.YearlyPay;

public class YearlyPayCommandHandler : CommandHandler, ICommandHandler<YearlyPayCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly TransactionYearAverageViewModelBuilder _averageViewModelBuilder;

    public YearlyPayCommandHandler(
        BudgetsClient budgetsClient,
        TransactionYearAverageViewModelBuilder averageViewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _averageViewModelBuilder = averageViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(YearlyPayCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        
        var budget =  budgets.First();
        
        var transactions = await budget.GetTransactions();
        
        var evaluator = new TransactionYearAverageAggregator(transactions);

        var viewModel = _averageViewModelBuilder
            .AddAggregator(evaluator)
            .AddColumnNames(TransactionYearAverageViewModel.GetColumnNames())
            .AddRowCount(false)
            .Build();

        return Compile(viewModel);
    }
}
