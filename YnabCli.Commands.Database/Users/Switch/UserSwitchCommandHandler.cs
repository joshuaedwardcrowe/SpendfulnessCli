using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using YnabCli.Database;

namespace YnabCli.Commands.Database.Users.Switch;

public class UserSwitchCommandHandler : CommandHandler, ICommandHandler<UserSwitchCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public UserSwitchCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ConsoleTable> Handle(UserSwitchCommand command, CancellationToken cancellationToken)
    {
        var currentActiveUser = await _dbContext.Users.FindActiveUserAsync();

        if (currentActiveUser.Name == command.UserName)
        {
            return CompileMessage($"User {command.UserName} already active.");
        }
        
        var switchingToUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == command.UserName, cancellationToken);
        
        if (switchingToUser == null)
        {
            return CompileMessage($"User {command.UserName} does not exist.");
        }
        
        currentActiveUser.Active = false;
        switchingToUser.Active = true;
        
        _dbContext.Users.Update(currentActiveUser);
        _dbContext.Users.Update(switchingToUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return CompileMessage($"User {command.UserName} active.");
    }
}