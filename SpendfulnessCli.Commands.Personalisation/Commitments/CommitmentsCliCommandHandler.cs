using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Aggregation.Aggregator;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using Spendfulness.Database.Sqlite.Commitments;
using Spendfulness.Database.Sqlite.Users;
using SpendfulnessCli.CliTables.ViewModelBuilders;

namespace SpendfulnessCli.Commands.Personalisation.Commitments;

public class CommitmentsCliCommandHandler(SpendfulnessDbContext dbContext, UserRepository userRepository) : CliCommandHandler, ICliCommandHandler<CommitmentsCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(CommitmentsCliCommand request, CancellationToken cancellationToken)
    {
        // Respect that there is a sync job.
        await dbContext.Sync<User>();
        await dbContext.Sync<Commitment>();
        
        var user = await userRepository.FindActiveUser();

        var aggregator = new CommitmentsYnabListAggregator(user.Commitments);
        
        var viewModel = new CommitmentsCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return OutcomeAs(viewModel);
    }
}