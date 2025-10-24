using Cli.Commands.Abstractions;
using Cli.Outcomes;
using YnabCli.Abstractions;
using YnabCli.Aggregation.Aggregator;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.Database.Commitments;
using YnabCli.Database.Users;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Personalisation.Commitments;

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