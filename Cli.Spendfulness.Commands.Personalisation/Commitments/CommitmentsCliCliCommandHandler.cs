using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Spendfulness.Database;
using Spendfulness.Database.Commitments;
using Spendfulness.Database.Users;

namespace Cli.Spendfulness.Commands.Personalisation.Commitments;

public class CommitmentsCliCliCommandHandler(SpendfulnessDb spendfulnessDb) : CliCommandHandler, ICliCommandHandler<CommitmentsCliCommand>
{
    public async Task<CliCommandOutcome> Handle(CommitmentsCliCommand request, CancellationToken cancellationToken)
    {
        // Respect that there is a sync job.
        await spendfulnessDb.Sync<User>();
        await spendfulnessDb.Sync<Commitment>();
        
        var user = await spendfulnessDb.GetActiveUser();

        var aggregator = new CommitmentsYnabAggregator(user.Commitments);
        
        var viewModel = new CommitmentsCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}