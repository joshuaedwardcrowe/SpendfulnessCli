using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;

namespace SpendfulnessCli.Commands.Personalisation.Users.Active;

public class UserActiveCliCommandHandler : CliCommandHandler, ICliCommandHandler<UserActiveCliCommand>
{
    private readonly SpendfulnessDbContext _dbContext;

    public UserActiveCliCommandHandler(SpendfulnessDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CliCommandOutcome> Handle(UserActiveCliCommand request, CancellationToken cancellationToken)
    {
        var activeUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Active);
        
        return activeUser != null
            ? Compile($"Active user is {activeUser.Name}")
            : Compile($"No active user.");
    }
}