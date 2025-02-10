using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Ynab;
using Ynab.Aggregates;
using Ynab.Clients;
using Ynab.Extensions;
using YnabProgressConsole.Compilation;
using YnabProgressConsole.Compilation.SpareMoneyView;

namespace YnabProgressConsole.Commands.SpareMoney;

public class SpareMoneyCommandHandler : CommandHandler, ICommandHandler<SpareMoneyCommand>
{
    private readonly BudgetsClient _budgetsClient;
    private readonly IAggregateViewModelBuilder<SpareMoneyAggregation, AccountBalanceAggregate> _aggregateViewModelBuilder;

    public SpareMoneyCommandHandler(BudgetsClient budgetsClient, 
        [FromKeyedServices(typeof(SpareMoneyAggregation))]
        IAggregateViewModelBuilder<SpareMoneyAggregation, AccountBalanceAggregate> aggregateViewModelBuilder)
    {
        _budgetsClient = budgetsClient;
        _aggregateViewModelBuilder = aggregateViewModelBuilder;
    }

    public async Task<ConsoleTable> Handle(SpareMoneyCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();

        var budget = budgets.First();
        
        var criticalSpendingAmount = await GetCriticalSpending(budget);
        
        var aggregation =await GetAggregation(budget, criticalSpendingAmount);

        var viewModel = _aggregateViewModelBuilder
            .AddAggregation(aggregation)
            .AddColumnNames(SpareMoneyViewModel.GetColumnNames())
            .Build();
        
        return Compile(viewModel);
    }
    
    private async Task<decimal> GetCriticalSpending(Budget budget)
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
        
        return categoryGroups
            .FilterTo(criticalCategoryGroupNames.ToArray())
            .Sum(o => o.Balance);
    }

    private async Task<SpareMoneyAggregation> GetAggregation(Budget budget, decimal criticalSpending)
    {
        var accounts = await budget.GetAccounts();
        
        return accounts
            .FilterToChecking()
            .AggregateByBalance()
            .IncludeAmountToIgnore(criticalSpending);
    }
}