namespace Cli.Commands.Abstractions.Artefacts;

public class ValuedCliCommandArtefact<TCommandPropertyValue>(TCommandPropertyValue artefactValue) : CliCommandArtefact
{
    public TCommandPropertyValue ArtefactValue { get; set; } = artefactValue;
}