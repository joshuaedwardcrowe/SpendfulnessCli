using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;

namespace Cli.Spendfulness.Commands.Personalisation.Users.Switch;

public class UserSwitchCliCliCommandHandler : CliCommandHandler, ICliCommandHandler<UserSwitchCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public UserSwitchCliCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(UserSwitchCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var currentActiveUser = await _dbContext.Users.FirstAsync(u => u.Active, cancellationToken);

        if (currentActiveUser.Name == cliCommand.UserName)
        {
            return Compile($"User {cliCommand.UserName} already active.");
        }
        
        var switchingToUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == cliCommand.UserName, cancellationToken);
        
        if (switchingToUser == null)
        {
            return Compile($"User {cliCommand.UserName} does not exist.");
        }
        
        currentActiveUser.Active = false;
        switchingToUser.Active = true;
        
        _dbContext.Users.Update(currentActiveUser);
        _dbContext.Users.Update(switchingToUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Compile($"User {cliCommand.UserName} active.");
    }
}