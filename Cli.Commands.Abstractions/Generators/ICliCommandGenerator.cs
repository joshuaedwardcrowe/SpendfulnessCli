namespace Cli.Commands.Abstractions.Generators;

// ReSharper disable once UnusedTypeParameter
public interface ICliCommandGenerator<TCommand> : IUnidentifiedCliCommandGenerator where TCommand : CliCommand
{
    // TODO: This might need to evolve into a 'CommandGeneratorStrategy' pattern of some kind.
    // This is helping us with reflection for DI.
}