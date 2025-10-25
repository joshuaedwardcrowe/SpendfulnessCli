using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Spendfulness.Commands.Personalisation.Users;

public class UserCliCommandHandler : ICliCommandHandler<UserCliCommand>
{
    public Task<CliCommandOutcome> Handle(UserCliCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}