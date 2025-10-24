using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;
using ConsoleTables;

namespace Cli.Spendfulness.Commands.Personalisation.Users;

public class UserCommandHandler : ICommandHandler<UserCommand>
{
    public Task<CliCommandOutcome> Handle(UserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}