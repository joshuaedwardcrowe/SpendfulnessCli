using Cli.Instructions.Parsers;
using Cli.Workflow.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Cli.Workflow;

public class CliWorkflow
{
    private readonly IServiceProvider _serviceProvider;
    private List<CliWorkflowRun> _workflowRuns = [];
    
    public CliWorkflowState State = CliWorkflowState.Started;

    public CliWorkflow(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public CliWorkflowRun CreateRun()
    {
        var state = new CliWorkflowRunState();
        
        var consoleInstructionParser = _serviceProvider.GetRequiredService<ConsoleInstructionParser>();
        
        var commandProvider = _serviceProvider.GetRequiredService<CliWorkflowCommandProvider>();
        
        // TODO: I'd like to remove the dependency on MediatR one day.
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        
        // TODO: CLI - I dont want all of these services hanging in the run forever? 
        var run = new CliWorkflowRun(state, consoleInstructionParser, commandProvider, mediator);
        
        _workflowRuns.Add(run);

        return run;
    }

    public void Stop()
    {
        State = CliWorkflowState.Stopped;
    }
}