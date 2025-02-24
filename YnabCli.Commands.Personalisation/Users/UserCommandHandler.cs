using ConsoleTables;

namespace YnabCli.Commands.Personalisation.Users;

public class UserCommandHandler : ICommandHandler<UserCommand>
{
    public Task<ConsoleTable> Handle(UserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}