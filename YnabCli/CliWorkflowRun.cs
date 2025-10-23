using System.Diagnostics;
using Cli.Instructions.Parsers;
using MediatR;

namespace YnabCli;

public class CliWorkflowRun
{
    private readonly CliIo _cliIO;
    private readonly ConsoleInstructionParser _consoleInstructionParser;
    private readonly CliCommandProvider _commandProvider;
    private readonly IMediator _mediator;
    
    private DateTime _startTime;
    private Stopwatch _stopwatch;
    
    public ClIWorkflowRunState State { get; private set; }

    public CliWorkflowRun(
        CliIo cliIo,
        ConsoleInstructionParser consoleInstructionParser,
        CliCommandProvider commandProvider,
        IMediator mediator)
    {
        _cliIO = cliIo;
        _consoleInstructionParser = consoleInstructionParser;
        _commandProvider = commandProvider;
        _mediator = mediator;

        _startTime = DateTime.Now;
        _stopwatch = new Stopwatch();
        
        State = ClIWorkflowRunState.NotYetRunning;
    }

    public async Task Run()
    {
        _stopwatch.Start();
        
        var input = _cliIO.Ask();
        if (string.IsNullOrEmpty(input))
        {
            _cliIO.Say($"Command '{input}' not found");
            UpdateState(ClIWorkflowRunState.NoInput);
            return;
        }
        
        var instruction = _consoleInstructionParser.Parse(input);

        try
        {
            UpdateState(ClIWorkflowRunState.Running);

            var command = _commandProvider.GetCommand(instruction);
            
            var table = await _mediator.Send(command);
        }
        catch (Exception e)
        {
            // TODO: Split out exceptions into: & add io.says
            // TODO: 1. NoCommandGeneratorException.

            UpdateState(ClIWorkflowRunState.Exceptional);
            return;
        }
        finally
        {
            _stopwatch.Stop();
        }
    }
    
    private void UpdateState(ClIWorkflowRunState newState)
    {
        if (State == newState)
        {
            // Already in that state, no mutation needed.
            return;
        }

        if (State == ClIWorkflowRunState.NotYetRunning && newState == ClIWorkflowRunState.Running)
        {
            State = newState;
        }

        if (State == ClIWorkflowRunState.Running && newState == ClIWorkflowRunState.Stopped)
        {
            State = newState;
        }

        throw new Exception($"Invalid Workflow state transition: {State} > {newState}");
    }
}