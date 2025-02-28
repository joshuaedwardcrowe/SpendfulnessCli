using Ynab;
using Ynab.Clients;
using Ynab.Connected;
using Ynab.Http;
using YnabCli.Database;

namespace YnabCli.Commands.Factories;

public class CommandBudgetGetter(UnitOfWork unitOfWork, YnabHttpClientBuilder httpClientBuilder)
{
    public async Task<ConnectedBudget> Get()
    {
        var activeUser = await unitOfWork.GetActiveUser();
        if (activeUser == null)
        {
            throw new Exception("No active user");
        }
        
        if (activeUser.YnabApiToken is null)
        {
            throw new Exception("No ynab api token");
        }
        
        var builder = httpClientBuilder.WithBearerToken(activeUser.YnabApiToken);
        var budgetClient = new BudgetsClient(builder);
        
        var budgets = await budgetClient.GetBudgets();
        
        if (activeUser.DefaultBudgetId is null)
        {
            return budgets.First();
        }
        
        return budgets.First(budget => budget.Id == activeUser.DefaultBudgetId);
    }
}