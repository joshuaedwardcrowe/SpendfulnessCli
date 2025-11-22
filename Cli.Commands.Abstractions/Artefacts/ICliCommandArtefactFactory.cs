using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Commands.Abstractions.Artefacts;

public interface ICliCommandArtefactFactory
{
    bool CanCreateWhen(CliCommandOutcome outcome);
    
    CliCommandArtefact Create(CliCommandOutcome outcome);
}