using Microsoft.Extensions.Hosting;
using Ynab;
using Ynab.Connected;
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
        var categoryGroups = await budget.GetCategoryGroups();
        var farmCategoryGroups = categoryGroups
            .FilterToFarmCategories()
            .Cast<ConnectedCategoryGroup>();

        foreach (var farmCategoryGroup in farmCategoryGroups)
        {
            var categories = farmCategoryGroup
                .Categories
                .Where(category => category.HasGoal);
            
            SyncCommitment(user, categories);
        }
        
        await unitOfWork.Save();
        
        Console.WriteLine("Milestone indexing worker ran...");
    }

    private void SyncCommitment(User user, IEnumerable<Category> categories)
    {
        foreach (var category in categories)
        {
            var commitment = user.Commitments
                .FirstOrDefault(commitment => commitment.YnabCategoryId == category.Id);

            if (commitment != null)
            {
                UpdateCommitment(commitment, category);

                continue;
            }
                
            AddCommitmentToUser(user, category);
        }
    }

    private void UpdateCommitment(Commitment commitment, Category category)
    {
        commitment.Name = category.Name;
        commitment.Funded = category.GoalOverallFunded.Value;
        commitment.Needed = category.GoalOverallLeft.Value;
        commitment.Started = category.GoalCreationMonth.Value;
        commitment.RequiredBy = category.GoalTargetMonth.Value;
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
}