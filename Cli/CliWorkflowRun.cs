using System.Diagnostics;
using Cli.Instructions.Parsers;
using Cli.Outcomes;
using Cli.Workflow.Abstractions;
using MediatR;
using YnabCli;

namespace Cli;

public class CliWorkflowRun
{
    private readonly CliWorkflowRunStateManager _stateManager;
    private readonly ConsoleInstructionParser _consoleInstructionParser;
    private readonly CliCommandProvider _commandProvider;
    private readonly IMediator _mediator;
    private readonly Stopwatch _stopwatch;

    public CliWorkflowRun(
        CliWorkflowRunStateManager stateManager,
        ConsoleInstructionParser consoleInstructionParser,
        CliCommandProvider commandProvider,
        IMediator mediator)
    {
        _stateManager = stateManager;
        _consoleInstructionParser = consoleInstructionParser;
        _commandProvider = commandProvider;
        _mediator = mediator;
        _stopwatch = new Stopwatch();
    }

    public bool IsValidAsk(string? ask) => string.IsNullOrEmpty(ask);
    
    public async Task<CliCommandOutcome> RespondToAsk(string? ask)
    {
        _stopwatch.Start();
        
        if (!IsValidAsk(ask))
        {
            _stateManager.ChangeTo(ClIWorkflowRunState.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        
        var instruction = _consoleInstructionParser.Parse(ask!);

        try
        {
            _stateManager.ChangeTo(ClIWorkflowRunState.Running);

            var command = _commandProvider.GetCommand(instruction);

            return await _mediator.Send(command);
        }
        catch (NoInstructionException)
        {
            _stateManager.ChangeTo(ClIWorkflowRunState.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (NoCommandGeneratorException)
        {
            _stateManager.ChangeTo(ClIWorkflowRunState.InvalidAsk);
            return new CliCommandNothingOutcome();
        }
        catch (Exception exception)
        {
            _stateManager.ChangeTo(ClIWorkflowRunState.Exceptional);
            return new CliCommandExceptionOutcome(exception);
        }
        finally
        {
            _stateManager.ChangeTo(ClIWorkflowRunState.Finished);
            _stopwatch.Stop();
        }
    }
}