using ConsoleTables;
using YnabCli.Commands.Handlers;
using YnabCli.Database;
using YnabCli.Database.Commitments;
using YnabCli.Database.Users;
using YnabCli.ViewModels.Aggregator;
using YnabCli.ViewModels.ViewModelBuilders;

namespace YnabCli.Commands.Personalisation.Commitments;

public class CommitmentsCommandHandler(YnabCliDb ynabCliDb) : CommandHandler, ICommandHandler<CommitmentsCommand>
{
    public async Task<ConsoleTable> Handle(CommitmentsCommand request, CancellationToken cancellationToken)
    {
        // Respect that there is a sync job.
        await ynabCliDb.Sync<User>();
        await ynabCliDb.Sync<Commitment>();
        
        var user = await ynabCliDb.GetActiveUser();

        var aggregator = new CommitmentsAggregator(user.Commitments);
        
        var viewModel = new CommitmentsViewModelBuilder()
            .WithAggregator(aggregator)
            .Build();

        return Compile(viewModel);
    }
}