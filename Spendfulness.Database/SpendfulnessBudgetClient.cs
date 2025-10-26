using Spendfulness.Database.Abstractions;
using Spendfulness.Database.Users;
using Ynab.Clients;
using Ynab.Connected;
using Ynab.Http;

namespace Spendfulness.Database;

public class SpendfulnessBudgetClient
{
    private readonly UserRepository _userRepository;
    private readonly YnabHttpClientBuilder _httpClientBuilder;

    public SpendfulnessBudgetClient(UserRepository userRepository, YnabHttpClientBuilder httpClientBuilder)
    {
        _userRepository = userRepository;
        _httpClientBuilder = httpClientBuilder;
    }

    // TODO: Could this override the base?
    public async Task<ConnectedBudget> GetDefaultBudget()
    {
        var activeUser = await _userRepository.FindActiveUser();
        if (activeUser == null)
        {
            // TODO: Better exception message.
            throw new SpendfulnessDbException(
                SpendfulnessDbExceptionCode.CannotConfigureBudget,
                "No active user found");
        }
        
        if (activeUser.YnabApiKey is null)
        {
            // TODO: Better exception message.
            throw new SpendfulnessDbException(
                SpendfulnessDbExceptionCode.CannotConfigureBudget,
                $"No {nameof(User.YnabApiKey)} setting");
        }
        
        var builder = _httpClientBuilder.WithBearerToken(activeUser.YnabApiKey);
        var budgetClient = new BudgetsClient(builder);
        
        var budgets = await budgetClient.GetBudgets();
        
        return activeUser.DefaultBudgetId is null
            ? budgets.First()
            : budgets.First(budget => budget.Id == activeUser.DefaultBudgetId);
    }
}