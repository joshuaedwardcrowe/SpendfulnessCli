using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Spendfulness.Database;
using Microsoft.EntityFrameworkCore;

namespace Cli.Spendfulness.Commands.Personalisation.Users.Active;

public class UserActiveCommandHandler : CommandHandler, ICommandHandler<UserActiveCommand>
{
    private readonly YnabCliDbContext _dbContext;

    public UserActiveCommandHandler(YnabCliDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(UserActiveCommand request, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Active);
        
        return activeUser != null
            ? Compile($"Active user is {activeUser.Name}")
            : Compile($"No active user.");
    }
}