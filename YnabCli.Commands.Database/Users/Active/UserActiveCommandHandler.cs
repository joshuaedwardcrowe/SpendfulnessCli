using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using YnabCli.Database;

namespace YnabCli.Commands.Database.Users.Active;

public class UserActiveCommandHandler : CommandHandler, ICommandHandler<UserActiveCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public UserActiveCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ConsoleTable> Handle(UserActiveCommand request, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Active);
        
        return activeUser != null
            ? CompileMessage($"Active user is {activeUser.Name}")
            : CompileMessage($"No active user.");
    }
}