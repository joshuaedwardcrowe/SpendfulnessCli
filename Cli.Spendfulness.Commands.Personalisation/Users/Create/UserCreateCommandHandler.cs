using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Database;
using Cli.Spendfulness.Database.Users;
using Microsoft.EntityFrameworkCore;

namespace Cli.Spendfulness.Commands.Personalisation.Users.Create;

public class UserCreateCommandHandler : CommandHandler, ICommandHandler<UserCreateCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public UserCreateCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(UserCreateCommand command, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Active, cancellationToken);
        
        var user = new User
        {
            Name = command.UserName,
            Active = activeUser == null 
        };
        
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Compile($"Created User \"{command.UserName}\".");
    }
}