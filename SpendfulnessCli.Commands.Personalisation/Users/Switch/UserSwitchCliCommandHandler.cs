using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;

namespace SpendfulnessCli.Commands.Personalisation.Users.Switch;

public class UserSwitchCliCommandHandler : CliCommandHandler, ICliCommandHandler<UserSwitchCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public UserSwitchCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome[]> Handle(UserSwitchCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var currentActiveUser = await _dbContext.Users.FirstAsync(u => u.Active, cancellationToken);

        if (currentActiveUser.Name == cliCommand.UserName)
        {
            return OutcomeAs($"User {cliCommand.UserName} already active.");
        }
        
        var switchingToUser = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == cliCommand.UserName, cancellationToken);
        
        if (switchingToUser == null)
        {
            return OutcomeAs($"User {cliCommand.UserName} does not exist.");
        }
        
        currentActiveUser.Active = false;
        switchingToUser.Active = true;
        
        _dbContext.Users.Update(currentActiveUser);
        _dbContext.Users.Update(switchingToUser);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return OutcomeAs($"User {cliCommand.UserName} active.");
    }
}