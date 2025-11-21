using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Commands.Abstractions.Properties;

public interface ICliCommandPropertyFactory
{
    bool CanCreatePropertyWhen(CliCommandOutcome outcome);
    
    CliCommandProperty CreateProperty(CliCommandOutcome outcome);
}