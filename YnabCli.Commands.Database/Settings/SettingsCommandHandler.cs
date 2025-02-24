using ConsoleTables;

namespace YnabCli.Commands.Database.Settings;

public class SettingsCommandHandler : ICommandHandler<SettingsCommand>
{
    public Task<ConsoleTable> Handle(SettingsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}