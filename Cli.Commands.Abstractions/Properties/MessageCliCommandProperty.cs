namespace Cli.Commands.Abstractions.Properties;

public class MessageCliCommandProperty(string message) : ValuedCliCommandProperty<string>(message)
{
}