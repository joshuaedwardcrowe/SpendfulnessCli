using Cli.Commands.Abstractions.Outcomes;
using MediatR;

namespace Cli.Commands.Abstractions.Handlers;

// TODO: I wonder if I could add an ICliCommandValidator.

public interface ICliCommandHandler<in TCommand> : IRequestHandler<TCommand, CliCommandOutcome[]> where TCommand : CliCommand
{
}