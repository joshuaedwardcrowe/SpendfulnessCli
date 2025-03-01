using Ynab.Clients;
using Ynab.Connected;
using Ynab.Http;
using YnabCli.Database;
using YnabCli.Database.Users;

namespace YnabCli.Commands.Factories;

public class CommandBudgetGetter(YnabCliDb ynabCliDb, YnabHttpClientBuilder httpClientBuilder)
{
    public async Task<ConnectedBudget> Get()
    {
        var activeUser = await ynabCliDb.GetActiveUser();
        if (activeUser == null)
        {
            throw new YnabCliDbException(YnabCliDbExceptionCode.DataNotFound, "No active user found");
        }
        
        if (activeUser.YnabApiKey is null)
        {
            throw new YnabCliDbException(YnabCliDbExceptionCode.DataNotFound, $"No {nameof(User.YnabApiKey)} setting");
        }
        
        var builder = httpClientBuilder.WithBearerToken(activeUser.YnabApiKey);
        var budgetClient = new BudgetsClient(builder);
        
        var budgets = await budgetClient.GetBudgets();
        
        if (activeUser.DefaultBudgetId is null)
        {
            return budgets.First();
        }
        
        return budgets.First(budget => budget.Id == activeUser.DefaultBudgetId);
    }
}