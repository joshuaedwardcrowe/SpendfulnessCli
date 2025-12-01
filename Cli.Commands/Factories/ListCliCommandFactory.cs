using Cli.Commands.Abstractions.Artefacts;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Arguments;

namespace Cli.Commands.Factories;

public abstract class ListCliCommandFactory
{
    protected static (int? pageSize, int? pageNumber) GetPaging(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var pageSizeArtefact = artefacts
            .OfType<int>(ListCliCommand.ArtefactNames.PageSize);

        var pageNumberArtefact = artefacts
            .OfType<int>(ListCliCommand.ArtefactNames.PageNumber);

        var pageSizeArgument = instruction
            .Arguments
            .OfType<int>(ListCliCommand.ArgumentNames.PageSize);

        var pageNumberArgument = instruction
            .Arguments
            .OfType<int>(ListCliCommand.ArgumentNames.PageNumber);

        var pageSize = pageSizeArgument?.ArgumentValue ?? pageSizeArtefact?.ArtefactValue;
        var pageNumber = pageNumberArgument?.ArgumentValue ?? pageNumberArtefact?.ArtefactValue;

        return (pageSize, pageNumber);
    }
}