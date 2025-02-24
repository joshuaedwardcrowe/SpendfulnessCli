using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using YnabCli.Database;

namespace YnabCli.Commands.Database.Settings.Create;

public class SettingCreateCommandHandler : CommandHandler, ICommandHandler<SettingCreateCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public SettingCreateCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ConsoleTable> Handle(SettingCreateCommand command, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FindActiveUserAsync();
        
        var type = await _dbContext.SettingTypes.FirstOrDefaultAsync(s => s.Name == command.Type);
        if (type == null)
        {
            throw new Exception($"{type} is not a Setting");
        }
        
        var setting = new YnabCli.Database.Settings.Setting
        {
            Type = type,
            Value = command.Value,
            User = activeUser,
        };
        
        await _dbContext.Settings.AddAsync(setting, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return CompileMessage($"Added setting: {command.Type} for user {activeUser.Name}");
    }
}