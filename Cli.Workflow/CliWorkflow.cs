using Cli.Instructions.Parsers;
using Cli.Workflow.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow;

/// <summary>
/// State machine of a command line interface.
/// </summary>
public class CliWorkflow
{
    private readonly IServiceProvider _serviceProvider;
    // TODO: CLI - Export this?
    // ReSharper disable once CollectionNeverQueried.Local
    private List<CliWorkflowRun> _runs = [];
    
    public CliWorkflowStatus Status = CliWorkflowStatus.Started;

    public CliWorkflow(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Create a new run, a sub-state machine of an individual execution.
    /// </summary>
    /// <returns>A sub-state mchine of an individual execution.</returns>
    public CliWorkflowRun CreateRun()
    {
        // TODO: CLI - Store this somewhere?
        var state = new CliWorkflowRunState();
        
        var instructionParser = _serviceProvider.GetRequiredService<CliInstructionParser>();
        
        var commandProvider = _serviceProvider.GetRequiredService<CliWorkflowCommandProvider>();
        
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        
        var run = new CliWorkflowRun(state, instructionParser, commandProvider, mediator);
        
        _runs.Add(run);

        return run;
    }

    /// <summary>
    /// Close the state machine.
    /// </summary>
    public void Stop()
    {
        Status = CliWorkflowStatus.Stopped;
    }
}