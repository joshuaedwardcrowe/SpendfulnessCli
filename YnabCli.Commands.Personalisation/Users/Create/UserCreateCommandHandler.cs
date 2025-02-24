using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using YnabCli.Database;

namespace YnabCli.Commands.Personalisation.Users.Create;

public class UserCreateCommandHandler : CommandHandler, ICommandHandler<UserCreateCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public UserCreateCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ConsoleTable> Handle(UserCreateCommand command, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Active, cancellationToken);
        
        var user = new YnabCli.Database.Users.User
        {
            Name = command.UserName,
            Active = activeUser == null 
        };
        
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return CompileMessage($"Created User \"{command.UserName}\".");
    }
}