using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Aggregation.Aggregator;
using Cli.Spendfulness.CliTables.ViewModelBuilders;
using Cli.Spendfulness.Database;
using Cli.Spendfulness.Database.Commitments;
using Cli.Spendfulness.Database.Users;

namespace Cli.Spendfulness.Commands.Personalisation.Commitments;

public class CommitmentsCommandHandler(YnabCliDb ynabCliDb) : CommandHandler, ICommandHandler<CommitmentsCommand>
{
    public async Task<CliCommandOutcome> Handle(CommitmentsCommand request, CancellationToken cancellationToken)
    {
        // Respect that there is a sync job.
        await ynabCliDb.Sync<User>();
        await ynabCliDb.Sync<Commitment>();
        
        var user = await ynabCliDb.GetActiveUser();

        var aggregator = new CommitmentsYnabAggregator(user.Commitments);
        
        var viewModel = new CommitmentsCliTableBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}