using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using Spendfulness.Database.Sqlite.Settings;

namespace SpendfulnessCli.Commands.Personalisation.Settings.Create;

public class CreateSettingCliCommandHandler : CliCommandHandler, ICliCommandHandler<CreateSettingCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public CreateSettingCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome[]> Handle(CreateSettingCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstAsync(u => u.Active, cancellationToken);
        
        var type = await _dbContext.SettingTypes.FirstOrDefaultAsync(s => s.Name == cliCommand.Type, cancellationToken);
        if (type == null)
        {
            throw new Exception($"{type} is not a Setting");
        }
        
        var setting = new Setting
        {
            Type = type,
            Value = cliCommand.Value,
            User = activeUser
        };
        
        await _dbContext.Settings.AddAsync(setting, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return OutcomeAs($"Added setting: {cliCommand.Type} for user {activeUser.Name}");
    }
}