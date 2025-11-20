using Cli.Commands.Abstractions.Outcomes;
using Cli.Workflow;
using Cli.Workflow.Abstractions;

namespace Cli;

// TODO: Rename to something clever?
public abstract class CliApp
{
    private readonly ICliWorkflow _workflow;
    protected readonly ICliCommandOutcomeIo Io;

    protected CliApp(ICliWorkflow workflow, ICliCommandOutcomeIo io)
    {
        _workflow = workflow;
        Io = io;
    }
    
    public async Task Run()
    { 
        OnSessionStart();
        
        while (_workflow.Status != CliWorkflowStatus.Stopped)
        {
            var run = _workflow.CreateRun();
            
            OnRunCreated(run);
            
            var ask = Io.Ask();
            
            var runTask =  run.RespondToAsk(ask);
            
            OnRunStarted(run, ask);

            var outcomes = await runTask;
            
            Io.Say(outcomes);
            
            OnRunComplete(run, outcomes);
        }
        
        OnSessionEnd(_workflow.Runs);
    }

    protected virtual void OnSessionStart()
    {
    }

    protected virtual void OnRunCreated(CliWorkflowRun run)
    {
    }

    protected virtual void OnRunStarted(CliWorkflowRun run, string ask)
    {
    }

    protected virtual void OnRunComplete(CliWorkflowRun run, CliCommandOutcome[] outcomes)
    {
    }
    
    protected virtual void OnSessionEnd(List<CliWorkflowRun> runs)
    {
    }
}