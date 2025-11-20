using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Personalisation.Users;

public class UserCliCommandHandler : ICliCommandHandler<UserCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(UserCliCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}