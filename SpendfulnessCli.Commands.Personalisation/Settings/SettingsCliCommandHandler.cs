using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Outcomes;

namespace SpendfulnessCli.Commands.Personalisation.Settings;

public class SettingsCliCommandHandler : ICliCommandHandler<SettingsCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(SettingsCliCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}