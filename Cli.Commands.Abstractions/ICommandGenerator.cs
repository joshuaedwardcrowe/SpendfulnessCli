namespace Cli.Commands.Abstractions;

// ReSharper disable once UnusedTypeParameter
// TODO: Rename to CliCommandGenerator.
public interface ICommandGenerator<TCommand> : IGenericCommandGenerator where TCommand : ICliCommand
{
    // TODO: This might need to evolve into a 'CommandGeneratorStrategy' pattern of some kind.
    // This is helping us with reflection for DI.
}