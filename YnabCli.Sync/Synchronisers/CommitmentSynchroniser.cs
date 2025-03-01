using Microsoft.Extensions.Hosting;
using Ynab;
using Ynab.Extensions;
using YnabCli.Abstractions;
using YnabCli.Database;
using YnabCli.Database.Commitments;
using YnabCli.Database.Users;

namespace YnabCli.Sync.Synchronisers;

public class CommitmentSynchroniser(BudgetGetter budgetGetter, YnabCliDb db) : BackgroundService
{
    private const int DefaultSyncFrequency = 1;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var user = await db.GetActiveUser();

            var syncFrequency = user.SyncFrequency ?? DefaultSyncFrequency;
            var syncFrequencyInMilliseconds = syncFrequency * 60 * 60 * 60;
            
            await SynchroniseCommitments(user);
            
            // Wait 5s until 
            await Task.Delay(500, stoppingToken);
        }
    }

    private async Task SynchroniseCommitments(User user)
    {
        var budget = await budgetGetter.Get();
        PrintToConsole($"Syncing Commitments for Budget: {budget.Id}");
        
        var categoryGroups = await budget.GetCategoryGroups();
        var farmCategoryGroups = categoryGroups
            .FilterToFarmCategories()
            .ToList();

        if (!farmCategoryGroups.Any())
        {
            throw new YnabCliException("No Farm Category Groups");
        }
        
        var categories = farmCategoryGroups
            .SelectMany(cg => cg.Categories)
            .ToList();

        var clone = new List<Commitment>(user.Commitments.ToList());

        foreach (var commitment in clone)
        {
            var category = categories.FirstOrDefault(cg => cg.Name == commitment.Name);
            
            PerformSync(user, commitment, category);
        }

        foreach (var category in categories)
        {
            var commitment = user.Commitments.FirstOrDefault(c => c.YnabCategoryId == category.Id);
            
            PerformSync(user, commitment, category);
        }
        
        PrintToConsole($"Finalising Sync...");
        await db.Save();
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
            Funded = category.GoalOverallFunded.Value,
            Needed = category.GoalOverallLeft.Value,
            Started = category.GoalCreationMonth.Value,
            RequiredBy = category.GoalTargetMonth.Value,
        };
                    
        user.Commitments.Add(commitment);
    }


    private static void UpdateCommitment(Commitment commitment, Category category)
    {
        commitment.Name = category.Name;
        commitment.Funded = category.GoalOverallFunded.Value;
        commitment.Needed = category.GoalOverallLeft.Value;
        commitment.Started = category.GoalCreationMonth.Value;
        commitment.RequiredBy = category.GoalTargetMonth.Value;
    }
    
    private void PrintToConsole(string message)
        => Console.WriteLine($"[{nameof(CommitmentSynchroniser)}] - {message}");
}