using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Clients;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.AmountByYear;

namespace YnabProgressConsole.Commands.SalaryIncreases;

public class SalaryIncreasesCommandHandler : CommandHandler, ICommandHandler<SalaryIncreasesCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IViewModelBuilder<AmountByYear> _viewModelBuilder;

    public SalaryIncreasesCommandHandler(
        BudgetsClient budgetsClient, 
        [FromKeyedServices(typeof(AmountByYear))]
        IViewModelBuilder<AmountByYear> viewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _viewModelBuilder = viewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(SalaryIncreasesCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        
        // TODO: Add support for selecting a budget if you ever do a settings feture.
        var budget =  budgets.First();
        
        var transactions = await budget.GetTransactions();
        
        var careerPayees = new List<string> { "BrightHR" };
        
        var monthlyPayByYear = transactions
            .FilterToInflow()
            .FilterByPayeeName(careerPayees.ToArray())
            .AverageByYear();

        var columnNames = AmountByYearViewModel.GetColumnNames();
        
        var viewModel = _viewModelBuilder
            .AddGroups(monthlyPayByYear)
            .AddColumnNames(columnNames.ToArray())
            .Build();

        return Compile(viewModel);
    }
}