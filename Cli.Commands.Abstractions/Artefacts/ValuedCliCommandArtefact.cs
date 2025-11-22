namespace Cli.Commands.Abstractions.Artefacts;

public class ValuedCliCommandArtefact<TCommandPropertyValue>(TCommandPropertyValue value) : CliCommandArtefact
{
    public TCommandPropertyValue Value { get; set; } = value;
}