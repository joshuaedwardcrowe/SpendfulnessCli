using Cli.Instructions.Parsers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using YnabCli;

namespace Cli;

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
        var stateManager = _serviceProvider.GetRequiredService<CliWorkflowRunStateManager>();
        
        var consoleInstructionParser = _serviceProvider.GetRequiredService<ConsoleInstructionParser>();
        
        var commandProvider = _serviceProvider.GetRequiredService<CliCommandProvider>();
        
        // TODO: I'd like to remove the dependency on MediatR one day.
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        
        // TODO: CLI - I dont want all of these services hanging in the run forever? 
        var run = new CliWorkflowRun(stateManager, consoleInstructionParser, commandProvider, mediator);
        
        _workflowRuns.Add(run);

        return run;
    }

    public void Stop()
    {
        State = CliWorkflowState.Stopped;
    }
}