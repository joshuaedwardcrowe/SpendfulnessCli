using Cli.Abstractions;
using Cli.Instructions.Abstractions;

namespace Cli.Commands.Abstractions;

public interface IUnidentifiedCliCommandGenerator
{
    CliCommand Generate(CliInstruction instruction);
}

// public interface IUnidentifiedContinuousCliCommandGenerator
// {
//     ContinuousCliCommand<TAggregate> Generate<TAggregate>(
//         CliInstruction instruction,
//         CliAggregator<TAggregate> aggregator);
// }