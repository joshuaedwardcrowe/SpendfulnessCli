using Cli.Spendfulness.Database;
using Cli.Spendfulness.Database.SpendingSamples;
using Microsoft.EntityFrameworkCore;

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
            .Select(sample => sample.Matches.Select(m => m.Id))
            .ToList();
        
        PrintToConsole($"Found {existingCombinations.Count} existing combinations");
        
        var storedSampleMatchCriteria = await db.GetSpendingSampleMatchCriteria();

        var possibleCombinations = CreateAllPossibleCombinations(storedSampleMatchCriteria);
        PrintToConsole($"Found {possibleCombinations.Count} possible combinations");
        
        var unique = GetUniqueCombinations(existingCombinations, possibleCombinations)
            .ToList();
        
        PrintToConsole($"Found {unique.Count} new combinations");
        
        await ConvertCombinationsToSamples(storedSampleMatchCriteria, unique);
        
        // TODO: Store the combinations in db.
        PrintToConsole("Completed all possible combinations");
    }

    private static List<IEnumerable<Guid>> CreateAllPossibleCombinations(List<SpendingSampleMatchCriteria> storedSampleMatchCriteria)
    {
        var possibleCombinations = new List<IEnumerable<Guid>>();

        foreach (var matchCriteria in storedSampleMatchCriteria)
        {
            var otherSampleMatchCriteria = storedSampleMatchCriteria
                .Where(smc => smc.Id != matchCriteria.Id);
            
            var newCombination = new List<Guid> { matchCriteria.Id };
            foreach (var otherMatchCriteria in otherSampleMatchCriteria)
            {
                newCombination.Add(otherMatchCriteria.Id);
                possibleCombinations.Add(newCombination.AsEnumerable());
            }
        }
        
        return possibleCombinations;
    }

    private IEnumerable<IEnumerable<Guid>> GetUniqueCombinations(
        IEnumerable<IEnumerable<Guid>> existingCombinations, IEnumerable<IEnumerable<Guid>> possibleCombinations)
    {
        var existingCombinationsList = existingCombinations.ToList();
        
        foreach (var possibleCombination in possibleCombinations)
        {
            // This will always be true because poss combinations are made of existing matches... combined.
            var possibleCombinationAlreadyFound = existingCombinationsList
                .Any(existingCombination => existingCombination.SequenceEqual(possibleCombination));

            if (possibleCombinationAlreadyFound)
            {
                continue;
            }
            
            yield return possibleCombination;
        }
    }
    
    private async Task ConvertCombinationsToSamples(
        List<SpendingSampleMatchCriteria> matchCriteria, List<IEnumerable<Guid>> uniqueCombinations)
    {
        for (var i = 0; i < uniqueCombinations.Count; i++)
        {
            PrintToConsole($"Converting Unique Combination {i + 1}");
            var uniqueCombination = uniqueCombinations[i];
            
            var matches = matchCriteria
                .Where(mc => uniqueCombination.Contains(mc.Id))
                .ToList();

            var sample =  new SpendingSample
            {
                Created = DateTime.UtcNow,
                // TODO: This just reassigns the matched SampleId.
                Matches = matches,
            };
            
            db.Context.SpendingSamples.Add(sample);
            await db.Save();
        }
    }
}
