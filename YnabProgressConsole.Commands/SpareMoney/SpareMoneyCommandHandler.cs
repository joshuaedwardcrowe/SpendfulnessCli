using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab;
using Ynab.Clients;
using Ynab.Extensions;
using YnabProgressConsole.Compilation.Evaluators;
using YnabProgressConsole.Compilation.ViewModelBuilders;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandHandler : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IViewModelBuilder<CategoryDeductedBalanceAggregator, decimal> _viewModelBuilder;

    public SpareMoneyCommandHandler(BudgetsClient budgetsClient, 
        [FromKeyedServices(typeof(CategoryDeductedBalanceAggregator))]
        IViewModelBuilder<CategoryDeductedBalanceAggregator, decimal> viewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _viewModelBuilder = viewModelBuilder;
    }
    
    public async Task<ConsoleTable> Handle(SpareMoneyCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
    
        var budget = budgets.First();

        var accounts = await budget.GetAccounts();
        var checkingAccounts = accounts.FilterToChecking();
    
        var criticalCategoryGroups = await GetCriticalCategoryGroups(budget);
    
        var evaluator = new CategoryDeductedBalanceAggregator(
            checkingAccounts.ToList(), 
            criticalCategoryGroups.ToList());
    
        var viewModel = _viewModelBuilder
            .AddAggregator(evaluator)
            .AddColumnNames(["Spare Money"])
            .AddRowCount(false)
            .Build();
    
        return Compile(viewModel);
    }
    
    private async Task<IEnumerable<CategoryGroup>> GetCriticalCategoryGroups(Budget budget)
    {
        var criticalCategoryGroupNames = new List<string>
        {
            "Phone",
            "Career",
            "Owning a Home",
            "Maintaining a Home",
            "Credit Card Payments"
        };
        
        var categoryGroups = await budget.GetCategoryGroups();

        return categoryGroups.FilterTo(criticalCategoryGroupNames);
    }
}