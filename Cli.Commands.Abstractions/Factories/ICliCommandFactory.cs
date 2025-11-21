namespace Cli.Commands.Abstractions.Factories;

// ReSharper disable once UnusedTypeParameter
public interface ICliCommandFactory<TCommand> : IUnidentifiedCliCommandFactory where TCommand : CliCommand
{
    // TODO: This might need to evolve into a 'CommandGeneratorStrategy' pattern of some kind.
    // This is helping us with reflection for DI.
}