using System.Diagnostics;
using Cli.Commands.Abstractions;
using Cli.Instructions.Parsers;
using Cli.Workflow.Abstractions;
using MediatR;

namespace Cli.Workflow;

public class CliWorkflowRun
{
    private readonly CliWorkflowRunState _state;
    private readonly ConsoleInstructionParser _consoleInstructionParser;
    private readonly CliWorkflowCommandProvider _workflowCommandProvider;
    private readonly IMediator _mediator;
    private readonly Stopwatch _stopwatch;

    public CliWorkflowRun(
        CliWorkflowRunState state,
        ConsoleInstructionParser consoleInstructionParser,
        CliWorkflowCommandProvider workflowCommandProvider,
        IMediator mediator)
    {
        _state = state;
        _consoleInstructionParser = consoleInstructionParser;
        _workflowCommandProvider = workflowCommandProvider;
        _mediator = mediator;
        _stopwatch = new Stopwatch();
    }

    public bool IsValidAsk(string? ask) => !string.IsNullOrEmpty(ask);
    
    public async Task<CliCommandOutcome> RespondToAsk(string? ask)
    {
        _stopwatch.Start();
        _state.ChangeTo(ClIWorkflowRunStateType.Created);
        
        if (!IsValidAsk(ask))
        {
            _state.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }

        try
        {
            _state.ChangeTo(ClIWorkflowRunStateType.Running);
            
            var instruction = _consoleInstructionParser.Parse(ask!);

            var command = _workflowCommandProvider.GetCommand(instruction);

            return await _mediator.Send(command);
        }
        // TODO: CLI - Custom/re-use exception at some point.
        catch (ArgumentNullException)
        {
            _state.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (NoInstructionException)
        {
            _state.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (NoCommandGeneratorException)
        {
            _state.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (Exception exception)
        {
            _state.ChangeTo(ClIWorkflowRunStateType.Exceptional);
            return new CliCommandExceptionOutcome(exception);
        }
        finally
        {
            _state.ChangeTo(ClIWorkflowRunStateType.Finished);
            _stopwatch.Stop();
        }
    }
}