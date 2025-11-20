using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;
using Spendfulness.Database.Users;

namespace SpendfulnessCli.Commands.Personalisation.Users.Create;

public class UserCreateCliCommandHandler : CliCommandHandler, ICliCommandHandler<UserCreateCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public UserCreateCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome[]> Handle(UserCreateCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Active, cancellationToken);
        
        var user = new User
        {
            Name = cliCommand.UserName,
            Active = activeUser == null 
        };
        
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return OutcomeAs($"Created User \"{cliCommand.UserName}\".");
    }
}