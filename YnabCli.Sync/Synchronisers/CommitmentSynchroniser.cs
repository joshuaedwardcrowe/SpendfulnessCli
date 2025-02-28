using Microsoft.Extensions.Hosting;
using Ynab;
using Ynab.Extensions;
using YnabCli.Database;
using YnabCli.Database.Commitments;
using YnabCli.Database.Users;

namespace YnabCli.Sync.Synchronisers;

public class CommitmentSynchroniser(BudgetGetter budgetGetter, UnitOfWork unitOfWork) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var user = await unitOfWork.GetActiveUser();
        var budget = await budgetGetter.Get();
        PrintToConsole($"Syncing Commitments for Budget: {budget.Id}");
        
        var categoryGroups = await budget.GetCategoryGroups();
        var farmCategoryGroups = categoryGroups
            .FilterToFarmCategories();

        foreach (var farmCategoryGroup in farmCategoryGroups)
        {
            SyncCommitment(user, farmCategoryGroup.Categories);
        }
        
        PrintToConsole($"Finalising Sync...");
        await unitOfWork.Save();
    }

    private void SyncCommitment(User user, IEnumerable<Category> categories)
    {
        foreach (var category in categories)
        {
            var commitment = user.Commitments
                .FirstOrDefault(commitment => commitment.YnabCategoryId == category.Id);
            
            if (commitment is not null && !category.HasGoal)
            {
                PrintToConsole($"Removing Commitment for: {category.Name}");
                user.Commitments.Remove(commitment);
            }
            
            if (commitment is not null && category.HasGoal)
            {
                PrintToConsole($"Updating Commitment for: {category.Name}");
                UpdateCommitment(commitment, category);

                continue;
            }
            
            if (!category.HasGoal)
            {
                PrintToConsole($"Skipping for: {category.Name}");
                continue;
            }
                
            PrintToConsole($"Adding a New Commitment for: {category.Name}");
            AddCommitmentToUser(user, category);
        }
    }
    
    private void AddCommitmentToUser(User user, Category category)
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


    private void UpdateCommitment(Commitment commitment, Category category)
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