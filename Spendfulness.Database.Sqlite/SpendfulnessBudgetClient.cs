using Spendfulness.Database.Abstractions;
using Spendfulness.Database.Sqlite.Users;
using Ynab.Clients;
using Ynab.Connected;
using Ynab.Factories;
using Ynab.Http;

namespace Spendfulness.Database.Sqlite;

// TODO: Write unit tests.
public class SpendfulnessBudgetClient
{
    private readonly UserRepository _userRepository;
    private readonly YnabHttpClientBuilder _httpClientBuilder;
    private readonly IEnumerable<ITransactionFactory> _transactionFactories;

    public SpendfulnessBudgetClient(
        UserRepository userRepository,
        YnabHttpClientBuilder httpClientBuilder,
        IEnumerable<ITransactionFactory> transactionFactories)
    {
        _userRepository = userRepository;
        _httpClientBuilder = httpClientBuilder;
        _transactionFactories = transactionFactories;
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
        var budgetClient = new BudgetsClient(builder, _transactionFactories);
        
        var budgets = await budgetClient.GetBudgets();
        
        return activeUser.DefaultBudgetId is null
            ? budgets.First()
            : budgets.First(budget => budget.Id == activeUser.DefaultBudgetId);
    }
}