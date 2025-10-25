using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Parsers;
using Cli.Workflow.Abstractions;
using MediatR;

namespace Cli.Workflow;

public class CliWorkflowRun
{
    private readonly CliWorkflowRunState _state;
    private readonly CliInstructionParser _cliInstructionParser;
    private readonly CliWorkflowCommandProvider _workflowCommandProvider;
    private readonly IMediator _mediator;

    public CliWorkflowRun(
        CliWorkflowRunState state,
        CliInstructionParser cliInstructionParser,
        CliWorkflowCommandProvider workflowCommandProvider,
        IMediator mediator)
    {
        _state = state;
        _cliInstructionParser = cliInstructionParser;
        _workflowCommandProvider = workflowCommandProvider;
        _mediator = mediator;
    }

    private bool IsValidAsk(string? ask) => !string.IsNullOrEmpty(ask);
    
    public async Task<CliCommandOutcome> RespondToAsk(string? ask)
    {
        _state.ChangeTo(ClIWorkflowRunStateType.Created);
        
        if (!IsValidAsk(ask))
        {
            _state.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }

        try
        {
            _state.ChangeTo(ClIWorkflowRunStateType.Running);
            
            var instruction = _cliInstructionParser.Parse(ask!);

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
        }
    }
}