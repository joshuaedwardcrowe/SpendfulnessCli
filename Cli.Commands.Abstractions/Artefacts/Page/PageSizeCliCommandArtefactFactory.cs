using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Outcomes.Reusable;
using Cli.Commands.Abstractions.Outcomes.Reusable.Page;

namespace Cli.Commands.Abstractions.Artefacts.Page;

public class PageSizeCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is PageSizeCliCommandOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        if (outcome is not PageSizeCliCommandOutcome pageSizeOutcome)
            throw new InvalidOperationException("Cannot create PageSizeCliCommandArtefact from the given outcome.");
        
        return new PageSizeCliCommandArtefact(pageSizeOutcome.PageSize);
    }
}