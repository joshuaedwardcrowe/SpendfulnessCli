using ConsoleTables;
using MediatR;

namespace YnabProgressConsole.Commands;

public interface ICommand : IRequest<ConsoleTable>
{
}