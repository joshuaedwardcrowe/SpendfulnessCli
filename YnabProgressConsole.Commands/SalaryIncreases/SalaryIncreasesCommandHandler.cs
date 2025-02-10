using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab.Clients;
using Ynab.Collections;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.ViewModelBuilders;
using YnabProgressConsole.Compilation.ViewModels;

namespace YnabProgressConsole.Commands.SalaryIncreases;

public class SalaryIncreasesCommandHandler : CommandHandler, ICommandHandler<SalaryIncreasesCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IGroupViewModelBuilder<AmountByYear> _groupViewModelBuilder;

    public SalaryIncreasesCommandHandler(
        BudgetsClient budgetsClient, 
        [FromKeyedServices(typeof(AmountByYear))]
        IGroupViewModelBuilder<AmountByYear> groupViewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _groupViewModelBuilder = groupViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(SalaryIncreasesCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
        
        var budget =  budgets.First();
        
        var transactions = await budget.GetTransactions();
        
        var careerPayees = new List<string> { "BrightHR" };
        
        var monthlyPayByYear = transactions
            .FilterToInflow()
            .FilterByPayeeName(careerPayees.ToArray())
            .AverageByYear();
        
        var viewModel = _groupViewModelBuilder
            .AddGroups(monthlyPayByYear)
            .AddColumnNames(AmountByYearViewModel.GetColumnNames())
            .AddRowCount(false)
            .Build();

        return Compile(viewModel);
    }
}