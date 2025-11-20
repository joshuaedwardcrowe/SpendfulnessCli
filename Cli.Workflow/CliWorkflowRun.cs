using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Instructions.Abstractions.Validators;
using Cli.Instructions.Parsers;
using Cli.Instructions.Validators;
using Cli.Workflow.Abstractions;
using MediatR;

namespace Cli.Workflow;

// TODO: Cli: I wonder if always attaching this to the command is a great way to add properties?
// And then let implementers of the CLI pass around properties between commands and hooks.
public class CliWorkflowRun
{
    public readonly CliWorkflowRunState State;
    
    private readonly ICliInstructionParser _cliInstructionParser;
    private readonly ICliInstructionValidator _cliInstructionValidator;
    private readonly ICliWorkflowCommandProvider _workflowCommandProvider;
    private readonly IMediator _mediator;

    public CliWorkflowRun(
        CliWorkflowRunState state,
        ICliInstructionParser cliInstructionParser,
        ICliInstructionValidator cliInstructionValidator,
        ICliWorkflowCommandProvider workflowCommandProvider,
        IMediator mediator)
    {
        State = state;
        
        _cliInstructionParser = cliInstructionParser;
        _cliInstructionValidator = cliInstructionValidator;
        _workflowCommandProvider = workflowCommandProvider;
        _mediator = mediator;
    }

    private bool IsEmptyAsk(string? ask) => !string.IsNullOrEmpty(ask);
    
    public async ValueTask<CliCommandOutcome[]> RespondToAsk(string? ask)
    {
        if (!IsEmptyAsk(ask))
        {
            State.ChangeTo(ClIWorkflowRunStateStatus.InvalidAsk);
            return [new CliCommandNothingOutcome()];
        }
        
        var instruction = _cliInstructionParser.Parse(ask!);
        
        if (_cliInstructionValidator.IsValidInstruction(instruction))
        {
            State.ChangeTo(ClIWorkflowRunStateStatus.Running, instruction);
        }
        else
        {
            State.ChangeTo(ClIWorkflowRunStateStatus.InvalidAsk);
            return [new CliCommandNothingOutcome()];
        }

        try
        {
            var command = _workflowCommandProvider.GetCommand(instruction);

            var outcomes = await _mediator.Send(command);
            
            UpdateStateAfterOutcome(outcomes);
            
            return outcomes;
        }
        catch (NoCommandGeneratorException)
        {
            State.ChangeTo(ClIWorkflowRunStateStatus.InvalidAsk);
            return [new CliCommandNothingOutcome()];
        }
        catch (Exception exception)
        {
            State.ChangeTo(ClIWorkflowRunStateStatus.Exceptional);
            return [new CliCommandExceptionOutcome(exception)];
        }
        finally
        {
            State.ChangeTo(ClIWorkflowRunStateStatus.Finished);
        }
    }

    private void UpdateStateAfterOutcome(CliCommandOutcome[] outcomes)
    {
        foreach (var outcome in outcomes)
        {
            State.ChangeTo(ClIWorkflowRunStateStatus.AchievedOutcome, outcome);
        }
    }
}