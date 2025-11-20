using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Spendfulness.Database;
using Spendfulness.Database.Commitments;
using Spendfulness.Database.Users;
using SpendfulnessCli.Aggregation.Aggregator;
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

        var aggregator = new CommitmentsYnabAggregator(user.Commitments);
        
        var viewModel = new CommitmentsCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return OutcomeAs(viewModel);
    }
}