using Cli.Abstractions;
using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Database;
using Microsoft.EntityFrameworkCore;

namespace Cli.Spendfulness.Commands.Personalisation.Settings.View;

public class SettingsVIewCommandHandler : CommandHandler, ICommandHandler<SettingsViewCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public SettingsVIewCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(SettingsViewCommand request, CancellationToken cancellationToken)
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
        
        return Compile(viewModel);
    }
}