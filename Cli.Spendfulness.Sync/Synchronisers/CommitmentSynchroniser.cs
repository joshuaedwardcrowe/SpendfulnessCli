using Cli.Abstractions;
using Spendfulness.Database;
using Spendfulness.Database.Commitments;
using Spendfulness.Database.Users;
using Ynab;
using Ynab.Extensions;

namespace YnabCli.Sync.Synchronisers;

public class CommitmentSynchroniser(SpendfulnessBudgetClient spendfulnessBudgetClient, UserRepository userRepository, SpendfulnessDbContext dbContext) : Synchroniser
{
    private const int DefaultSyncFrequency = 1;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var user = await userRepository.FindActiveUser();

            var syncFrequency = user.SyncFrequency ?? DefaultSyncFrequency;
            var syncFrequencyInMilliseconds = syncFrequency * 60 * 60 * 60;
            
            await SynchroniseCommitments(user);
            
            // Wait 5s until 
            await Task.Delay(syncFrequencyInMilliseconds, stoppingToken);
        }
    }

    private async Task SynchroniseCommitments(User user)
    {
        var budget = await spendfulnessBudgetClient.GetDefaultBudget();
        PrintToConsole($"Syncing Commitments for Budget: {budget.Id}");
        
        var categoryGroups = await budget.GetCategoryGroups();
        var farmCategoryGroups = categoryGroups
            .FilterToFarmCategories()
            .ToList();

        if (!farmCategoryGroups.Any())
        {
            throw new CliException(CliExceptionCode.Custom, "No Farm Category Groups");
        }
        
        var categories = farmCategoryGroups
            .SelectMany(cg => cg.Categories)
            .ToList();

        var categoryIds = farmCategoryGroups.SelectMany(cg => cg.GetCategoryIds());
        var commitmentsCategoryIds = user.Commitments.Select(c => c.YnabCategoryId);
        var allCategoryIds = categoryIds.Concat(commitmentsCategoryIds).Distinct();
        
        foreach (var categoryId in allCategoryIds)
        {
            var category = categories.FirstOrDefault(cg => cg.Id == categoryId);
            var commitment = user.Commitments.FirstOrDefault(c => c.YnabCategoryId == categoryId);
            
            PerformSync(user, commitment, category);
        }
        
        PrintToConsole($"Finalising Sync...");
        await dbContext.SaveChangesAsync();
    }

    private void PerformSync(User user, Commitment? commitment, Category? category)
    {
        if (commitment is null && category is { HasGoal: false })
        {
            PrintToConsole($"Skipping for: {category.Name}"); 

            return;
        }
        
        if (commitment is null && category is { HasGoal: true})
        {
            PrintToConsole($"Adding a New Commitment for: {category.Name}");
            AddCommitmentToUser(user, category);
        }

        if (commitment is not null && category is { HasGoal: true })
        {
            PrintToConsole($"Updating Commitment for: {category.Name}");
            UpdateCommitment(commitment, category);

            return;
        }
        
        if (commitment is not null && category is null)
        {
            PrintToConsole($"Removing Commitment for: {commitment.Name}");
            user.Commitments.Remove(commitment);
        }
    }
    
    private static void AddCommitmentToUser(User user, Category category)
    {
        var commitment = new Commitment
        {
            Name = category.Name,
            User = user,
            YnabCategoryId = category.Id,
            Funded = category.GoalOverallFunded,
            Needed = category.GoalOverallLeft,
            Started = category.GoalCreationMonth,
            RequiredBy = category.GoalTargetMonth,
        };
                    
        user.Commitments.Add(commitment);
    }


    private static void UpdateCommitment(Commitment commitment, Category category)
    {
        commitment.Name = category.Name;
        commitment.Funded = category.GoalOverallFunded;
        commitment.Needed = category.GoalOverallLeft;
        commitment.Started = category.GoalCreationMonth;
        commitment.RequiredBy = category.GoalTargetMonth;
    }
}