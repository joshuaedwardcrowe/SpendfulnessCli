using Cli.Commands.Abstractions.Outcomes;
using MediatR;

namespace Cli.Commands.Abstractions.Handlers;

// TODO: Make MediatR dependency not inheritable to other packages.
public interface ICliCommandHandler<in TCommand> : IRequestHandler<TCommand, CliCommandOutcome[]> where TCommand : CliCommand
{
}