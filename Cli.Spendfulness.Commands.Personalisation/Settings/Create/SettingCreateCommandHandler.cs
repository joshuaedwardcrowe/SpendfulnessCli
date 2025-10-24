using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Database;
using Cli.Spendfulness.Database.Settings;
using Microsoft.EntityFrameworkCore;

namespace Cli.Spendfulness.Commands.Personalisation.Settings.Create;

public class SettingCreateCommandHandler : CommandHandler, ICommandHandler<SettingCreateCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public SettingCreateCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(SettingCreateCommand command, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstAsync(u => u.Active, cancellationToken);
        
        var type = await _dbContext.SettingTypes.FirstOrDefaultAsync(s => s.Name == command.Type, cancellationToken);
        if (type == null)
        {
            throw new Exception($"{type} is not a Setting");
        }
        
        var setting = new Setting
        {
            Type = type,
            Value = command.Value,
            User = activeUser
        };
        
        await _dbContext.Settings.AddAsync(setting, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Compile($"Added setting: {command.Type} for user {activeUser.Name}");
    }
}