using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;
using Spendfulness.Database.SpendingSamples;

namespace Cli.Spendfulness.Commands.Personalisation.Samples.Matches;

public class SamplesMatchesCliCommandHandler(SpendfulnessBudgetClient spendfulnessBudgetClient, SpendfulnessDb db)
    : ICliCommandHandler<SamplesMatchesCliCommand>
{
    public async Task<CliCommandOutcome> Handle(SamplesMatchesCliCommand request, CancellationToken cancellationToken)
    {
        var budget = await spendfulnessBudgetClient.GetDefaultBudget();
        
        // Step 1: Get the transaction
        var transaction = await budget.GetTransaction(request.TransactionId);
        
        // Step 2: Find all sample matches -- flter by payee name, category name, memo.
        // TODO: Move to DB abstraction
        var samples = await db.Context.SpendingSamples
            .Include(x => x.Matches)
            .ThenInclude(x => x.Prices)
            .ToListAsync(cancellationToken);
        
        // TODO: Turn this into an aggregator?
        // TODO: Create an aggregate that is <sampleId, MatchCount>
        var _ = samples
            .Where(sample => sample.SimilarTo(transaction))
            .ToList();
        
        // Step 4: Show in a view model.
        
        throw new NotImplementedException();
    }
}