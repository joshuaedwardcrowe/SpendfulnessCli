using Cli.Commands.Abstractions.Handlers;
using Cli.Commands.Abstractions.Outcomes;
using Microsoft.EntityFrameworkCore;
using Spendfulness.Database;
using Spendfulness.Database.Sqlite;
using Spendfulness.Database.Sqlite.Users;

namespace SpendfulnessCli.Commands.Personalisation.Users.Create;

public class CreateUserCliCommandHandler(SpendfulnessDbContext dbContext)
    : CliCommandHandler, ICliCommandHandler<CreateUserCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(CreateUserCliCommand cliCommand, CancellationToken cancellationToken)
    {
        var activeUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Active, cancellationToken);
        
        var user = new User
        {
            Name = cliCommand.UserName,
            Active = activeUser == null 
        };
        
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return OutcomeAs($"Created User \"{cliCommand.UserName}\".");
    }
}