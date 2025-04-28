using Microsoft.EntityFrameworkCore;
using YnabCli.Database;

namespace YnabCli.Sync.Synchronisers;

public class PossibleSpendingSampleSynchroniser(YnabCliDb db) : Synchroniser
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var samples = await db.Context.SpendingSamples
            .Include(ss => ss.Matches)
            .ToListAsync();
        
        // Firs thing im thinking about is algorithm needs to be able to rule out existing combinations.
        var existingCombinations = samples
            .Select(sample => sample.Matches.Select(m => m.Id));

        var possibleCombinations = await CreateAllPossibleCombinations();
        
        // TODO: Store the combinations in db.
    }

    private async Task<List<IEnumerable<Guid>>> CreateAllPossibleCombinations()
    {
        var possibleCombinations = new List<IEnumerable<Guid>>();
        
        var storedSampleMatchCriteria = await db.GetSpendingSampleMatchCriteria();

        foreach (var matchCriteria in storedSampleMatchCriteria)
        {
            var otherSampleMatchCriteria = storedSampleMatchCriteria
                .Where(smc => smc.Id == matchCriteria.Id);
            
            var newCombination = new List<Guid> { matchCriteria.Id };
            foreach (var otherMatchCriteria in otherSampleMatchCriteria)
            {
                newCombination.Add(otherMatchCriteria.Id);
                possibleCombinations.Add(newCombination.AsEnumerable());
            }
        }
        
        return possibleCombinations;
    }
}
