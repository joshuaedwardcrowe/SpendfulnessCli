using Cli.Commands.Abstractions;
using Cli.Commands.Abstractions.Exceptions;
using Cli.Commands.Abstractions.Outcomes;
using Cli.Commands.Abstractions.Properties;
using Cli.Instructions.Abstractions;
using Cli.Instructions.Parsers;
using Cli.Workflow.Abstractions;
using MediatR;

namespace Cli.Workflow;

// TODO: Write unit tests.
// TODO: Cli: I wonder if always attaching this to the command is a great way to add properties?
// And then let implementers of the CLI pass around properties between commands and hooks.
public class CliWorkflowRun
{
    public readonly CliWorkflowRunState State;
    public readonly List<CliInstruction> Instructions;
    public readonly Dictionary<string, CliCommandProperty> Properties;
    
    private readonly CliInstructionParser _cliInstructionParser;
    private readonly CliWorkflowCommandProvider _workflowCommandProvider;
    private readonly IMediator _mediator;

    public CliWorkflowRun(
        CliWorkflowRunState state,
        List<CliInstruction> instructions,
        Dictionary<string, CliCommandProperty> properties,
        CliInstructionParser cliInstructionParser,
        CliWorkflowCommandProvider workflowCommandProvider,
        IMediator mediator)
    {
        State = state;
        Instructions = instructions;
        Properties = properties;

        _cliInstructionParser = cliInstructionParser;
        _workflowCommandProvider = workflowCommandProvider;
        _mediator = mediator;
    }

    private bool IsValidAsk(string? ask) => !string.IsNullOrEmpty(ask);
    
    public async Task<CliCommandOutcome> RespondToAsk(string? ask)
    {
        var needsToContinue = State.Is(ClIWorkflowRunStateType.CanContinue);
        
        if (!needsToContinue)
        {
            // If it's already running, its not created!
            State.ChangeTo(ClIWorkflowRunStateType.Created);
        }
        
        // Do process as normal.
        if (!IsValidAsk(ask))
        {
            State.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }

        try
        {
            State.ChangeTo(ClIWorkflowRunStateType.Running);
            
            var command = AskToCommand(ask!);
            
            return await ExecuteCommand(command);
        }
        // TODO: CLI - Custom/re-use exception at some point.
        catch (ArgumentNullException)
        {
            State.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (NoInstructionException)
        {
            State.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (NoCommandGeneratorException)
        {
            State.ChangeTo(ClIWorkflowRunStateType.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (Exception exception)
        {
            State.ChangeTo(ClIWorkflowRunStateType.Exceptional);
            return new CliCommandExceptionOutcome(exception);
        }
        finally
        {
            var command = AskToCommand(ask!);
            
            // TODO: Not necessarily a dictator of continuation.
            var nextState = command.IsContinuous
                ? ClIWorkflowRunStateType.CanContinue
                : ClIWorkflowRunStateType.Finished;
                        
            State.ChangeTo(nextState);
        }
    }

    private CliCommand AskToCommand(string ask)
    {
        var nextInstruction = _cliInstructionParser.Parse(ask);
        
        var priorInstruction = Instructions.LastOrDefault();
        
        var onSameInstruction = nextInstruction.Name == priorInstruction?.Name;
        
        // If the next instruction is different from the last,
        // It's a new instruction.
        if (!onSameInstruction)
        {
            Instructions.Add(nextInstruction);
        }
        
        var ignorePriorInstruction = priorInstruction == null || onSameInstruction;

        return ignorePriorInstruction
            ? _workflowCommandProvider.Provide(nextInstruction)
            : _workflowCommandProvider.Provide(priorInstruction!, nextInstruction);
    }

    private async Task<CliCommandOutcome> ExecuteCommand<TCliCommand>(TCliCommand command) where TCliCommand : CliCommand
    {
        command.Properties = Properties.Values.ToList();
        
        var outcome = await _mediator.Send(command);

        // Overwrite existing properties with new ones in case they were changed.
        foreach (var property in command.Properties)
        {
            Properties[property.PropertyKey] = property;
        }
        
        return outcome;
    }
}