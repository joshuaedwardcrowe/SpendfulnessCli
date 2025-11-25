using Cli.Abstractions;
using Cli.Abstractions.Tables;
using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;

namespace SpendfulnessCli.Commands.Personalisation.Settings.View;

public class ViewSettingCliCommandHandler : CliCommandHandler, ICliCommandHandler<ViewSettingCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public ViewSettingCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome[]> Handle(ViewSettingCliCommand request, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users
            .Include(user => user.Settings)
            .ThenInclude(setting => setting.Type)
            .FirstAsync(u => u.Active, cancellationToken);

        var rows = activeUser
            .Settings
            .Select(setting => new List<object>
            {
                setting.Type.Name,
                setting.Value
            })
            .ToList();

        var viewModel = new CliTable
        {
            Columns = ["Setting Type", "Setting Value"],
            Rows = rows
        };
        
        return OutcomeAs(viewModel);
    }
}