namespace Cli.Commands.Abstractions.Properties;

public class ValuedCliCommandProperty<TCommandPropertyValue>(TCommandPropertyValue value) : CliCommandProperty
{
    public TCommandPropertyValue Value { get; set; } = value;
}