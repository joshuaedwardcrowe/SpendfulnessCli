# SpendfulnessBudgetClient

## Premise
The SpendfulnessCli application needs to interact with YNAB budgets to perform various operations like reporting, synchronization, and data analysis. Users may have multiple budgets in their YNAB account, and manually specifying which budget to use for every operation would be cumbersome and error-prone.

## Problem
Without a centralized mechanism to determine which budget to use, every command handler would need to:
1. Request authentication credentials from the user
2. Fetch all available budgets
3. Determine which budget to operate on
4. Create the appropriate connected budget instance

This leads to code duplication and inconsistent behavior across different commands.

## Solution
The `SpendfulnessBudgetClient` class provides a centralized service that automatically resolves the default budget for the active user based on their stored settings. It encapsulates the logic for:

1. Retrieving the active user from the local database
2. Authenticating with the YNAB API using the user's stored API key
3. Fetching available budgets
4. Selecting the appropriate budget (either the user's specified default or the first available budget)

### Implementation Details
```csharp
public class SpendfulnessBudgetClient(SpendfulnessDb db, YnabHttpClientBuilder httpClientBuilder)
{
    public async Task<ConnectedBudget> GetDefaultBudget()
    {
        var activeUser = await db.GetActiveUser();
        var builder = httpClientBuilder.WithBearerToken(activeUser.YnabApiKey);
        var budgetClient = new BudgetsClient(builder);
        var budgets = await budgetClient.GetBudgets();
        
        return activeUser.DefaultBudgetId is null
            ? budgets.First()
            : budgets.First(budget => budget.Id == activeUser.DefaultBudgetId);
    }
}
```

### User Setup Instructions
To use SpendfulnessBudgetClient, users must complete the following setup steps:

#### 1. Create the Database
```bash
./database create
```
This initializes the local SQLite database that stores user profiles, settings, and custom data.

#### 2. Create a User Profile
```bash
./user create --user-name YourName
```
This creates a user profile and automatically sets it as the active user. If multiple users exist, you can switch between them:
```bash
./user switch --user-name OtherName
```

#### 3. Configure YNAB API Key
Obtain your personal access token from the [YNAB Developer Settings](https://app.ynab.com/settings/developer) page, then store it:
```bash
./settings create --type YnabApiKey --value your_api_key_here
```

The API key is stored securely in the local database and associated with the active user.

#### 4. (Optional) Set Default Budget
If you have multiple budgets in YNAB and want to specify which one to use by default:
```bash
./settings create --type DefaultBudgetId --value budget-guid-here
```

If no default budget is specified, SpendfulnessBudgetClient will automatically use the first budget returned by the YNAB API.

### How It Works
When any command handler requires access to budget data, it receives `SpendfulnessBudgetClient` as a dependency through dependency injection. The command handler calls `GetDefaultBudget()`, which:

1. Queries the database for the active user
2. Validates that the user has a YNAB API key configured
3. Authenticates with the YNAB API
4. Fetches all accessible budgets
5. Returns a `ConnectedBudget` instance for either:
   - The budget matching the user's `DefaultBudgetId` setting, or
   - The first budget if no default is configured

The returned `ConnectedBudget` is a [Connected API](./ADR05-Connected-API-Concept.md) object that provides convenient methods for accessing accounts, categories, transactions, and scheduled transactions without requiring additional client management.

### Integration with Dependency Injection
The `SpendfulnessBudgetClient` is registered as a singleton in the service container:
```csharp
services.AddSingleton<SpendfulnessBudgetClient>();
```

This allows it to be injected into command handlers:
```csharp
public class YearlySpendingCliCliCommandHandler(SpendfulnessBudgetClient budgetClient)
    : CliCommandHandler, ICliCommandHandler<YearlySpendingCliCommand>
{
    public async Task<CliCommandOutcome> Handle(YearlySpendingCliCommand request, CancellationToken cancellationToken)
    {
        var budget = await budgetClient.GetDefaultBudget();
        var transactions = await budget.GetTransactions();
        
        // Use budget and transactions to perform operations
    }
}
```

## Constraints

### Single Active User
The implementation assumes a single active user at a time. While multiple user profiles can exist in the database, only one can be active. Operations always use the active user's credentials.

### First Budget Fallback
When no default budget is specified, the client returns the first budget from the YNAB API response. The order of budgets in the API response is not guaranteed, so users with multiple budgets should explicitly configure their default.

### Local Storage Dependency
The solution requires a local SQLite database. The CLI cannot operate in a stateless mode; initial setup is required before any budget operations can be performed.

### API Key Security
API keys are stored in plaintext in the local SQLite database. Users should ensure appropriate file system permissions are set to protect the database file.

## Questions & Answers

### Why not prompt for the API key on each run?
Prompting for the API key on every command execution would significantly degrade the user experience, especially for automated workflows or scripting scenarios. Storing the key locally allows for seamless operation while maintaining security through file system permissions.

### Why use the first budget as the fallback instead of prompting the user?
Most YNAB users have only one budget. For users with multiple budgets, requiring them to explicitly configure their default is more reliable than interactive prompts, which don't work well in automated scenarios.

### Can I switch between budgets easily?
Yes, you can update the `DefaultBudgetId` setting at any time using the settings create command. This will affect all subsequent operations. Alternatively, you can switch between user profiles if different budgets are associated with different users.

### What happens if my API key expires or is revoked?
The `GetDefaultBudget()` method will throw an exception when authentication fails. Users will need to generate a new API key from the YNAB Developer Settings and update their configuration using the settings create command.

### How do I find my Budget ID?
You can find your budget ID in several ways:
1. Visit your budget in YNAB web app - the URL contains the budget ID
2. Use the YNAB API directly to list your budgets
3. Check the budget's settings page in the YNAB app

The budget ID is a GUID in the format: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx`
