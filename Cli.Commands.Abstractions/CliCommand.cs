using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using MediatR;

namespace Cli.Commands.Abstractions;


public abstract record ContinuousCliCommand() : CliCommand(isContinuous: true)
{
}