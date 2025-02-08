using ConsoleTables;
using MediatR;

namespace YnabProgressConsole.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, ConsoleTable> where TCommand : ICommand
{
    
}