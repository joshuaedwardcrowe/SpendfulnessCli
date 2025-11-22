using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Commands.Abstractions.Artefacts;

public interface ICliCommandArtefactFactory
{
    bool For(CliCommandOutcome outcome);
    
    CliCommandArtefact Create(CliCommandOutcome outcome);
}