using Cli.Instructions.Abstractions.Validators;
using Cli.Instructions.Parsers;
using Cli.Instructions.Validators;
using Cli.Workflow.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow;

// TODO: Write unit tests.
/// <summary>
/// State machine of a command line interface.
/// </summary>
public class CliWorkflow
{
    public List<CliWorkflowRun> Runs = [];
    
    private readonly IServiceProvider _serviceProvider;
    
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
        
        var instructionParser = _serviceProvider.GetRequiredService<ICliInstructionParser>();

        var instructionValidator = _serviceProvider.GetRequiredService<ICliInstructionValidator>();
        
        var commandProvider = _serviceProvider.GetRequiredService<ICliWorkflowCommandProvider>();
        
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        
        var run = new CliWorkflowRun(
            state,
            instructionParser,
            instructionValidator,
            commandProvider,
            mediator);
        
        Runs.Add(run);

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