using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Database;
using Cli.Spendfulness.Database.SpendingSamples;
using Microsoft.EntityFrameworkCore;

namespace Cli.Spendfulness.Commands.Personalisation.Samples.Matches;

public class SamplesMatchesCommandHandler(ConfiguredBudgetClient configuredBudgetClient, YnabCliDb db)
    : ICommandHandler<SamplesMatchesCommand>
{
    public async Task<CliCommandOutcome> Handle(SamplesMatchesCommand request, CancellationToken cancellationToken)
    {
        var budget = await configuredBudgetClient.GetDefaultBudget();
        
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