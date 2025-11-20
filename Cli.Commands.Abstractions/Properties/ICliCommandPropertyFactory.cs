using Cli.Commands.Abstractions.Outcomes;

namespace Cli.Commands.Abstractions.Properties;

public interface ICliCommandPropertyFactory
{
    bool CanCreateProperty(CliCommandOutcome outcome);
    
    CliCommandProperty CreateProperty(CliCommandOutcome outcome);
}