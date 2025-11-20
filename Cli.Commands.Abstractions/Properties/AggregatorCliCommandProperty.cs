using Cli.Abstractions;

namespace Cli.Commands.Abstractions.Properties;

public class AggregatorCliCommandProperty<TAggregate>(CliAggregator<TAggregate> value)
    : ValuedCliCommandProperty<CliAggregator<TAggregate>>(value)
{
    
}