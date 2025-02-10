using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab;
using Ynab.Clients;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.SpareMoneyView;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandHandler : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IEvaluationViewModelBuilder<CategoryDeductedBalanceEvaluator, decimal> _evaluationViewModelBuilder;

    public SpareMoneyCommandHandler(BudgetsClient budgetsClient, 
        [FromKeyedServices(typeof(CategoryDeductedBalanceEvaluator))]
        IEvaluationViewModelBuilder<CategoryDeductedBalanceEvaluator, decimal> evaluationViewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _evaluationViewModelBuilder = evaluationViewModelBuilder;
    }
    
    public async Task<ConsoleTable> Handle(SpareMoneyCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();
    
        var budget = budgets.First();

        var accounts = await budget.GetAccounts();
        var checkingAccounts = accounts.FilterToChecking();
    
        var criticalCategoryGroups = await GetCriticalCategoryGroups(budget);
    
        var evaluator = new CategoryDeductedBalanceEvaluator(checkingAccounts, criticalCategoryGroups);
    
        var viewModel = _evaluationViewModelBuilder
            .AddEvaluator(evaluator)
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