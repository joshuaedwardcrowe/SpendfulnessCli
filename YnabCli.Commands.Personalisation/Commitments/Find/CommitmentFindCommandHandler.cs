using ConsoleTables;
using Ynab.Clients;
using YnabCli.Database;

namespace YnabCli.Commands.Personalisation.Commitments.Find;

public class CommitmentFindCommandHandler : ICommandHandler<CommitmentFindCommand>
{
    private readonly YnabCliDbContext _dbContext;
    private readonly BudgetsClient _budgetsClient;

    public CommitmentFindCommandHandler(YnabCliDbContext dbContext, BudgetsClient budgetsClient)
    {
        _dbContext = dbContext;
        _budgetsClient = budgetsClient;
    }

    public async Task<ConsoleTable> Handle(CommitmentFindCommand request, CancellationToken cancellationToken)
    {
        var budgets = await _budgetsClient.GetBudgets();

        var budget = budgets.First();

        return new ConsoleTable();
    }
}