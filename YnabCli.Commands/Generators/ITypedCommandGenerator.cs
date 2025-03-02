namespace YnabCli.Commands.Generators;

// ReSharper disable once UnusedTypeParameter
public interface ITypedCommandGenerator<TCommand> where TCommand : ICommand
{
    // This is helping us with reflection for DI.
}