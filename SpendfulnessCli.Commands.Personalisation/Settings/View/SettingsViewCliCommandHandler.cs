using Cli.Abstractions;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;

namespace SpendfulnessCli.Commands.Personalisation.Settings.View;

public class SettingsViewCliCommandHandler : CliCommandHandler, ICliCommandHandler<SettingsViewCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public SettingsViewCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome[]> Handle(SettingsViewCliCommand request, CancellationToken cancellationToken)
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